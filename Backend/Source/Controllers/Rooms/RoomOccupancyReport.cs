#region

using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils.Auth;
using DB = PoliNetwork.Db.Utils.Database;

#endregion

namespace PoliFemoBackend.Source.Controllers.Rooms;

[ApiController]
[ApiExplorerSettings(GroupName = "Rooms")]
[Route("/rooms/{id:int}/occupancy")]
public class RoomOccupancyReport : ControllerBase
{
    /// <summary>
    ///     Send a report about the occupancy of a room
    /// </summary>
    /// <remarks>
    ///     The rate must be between 1 and 5
    /// </remarks>
    /// <param name="id">Room ID</param>
    /// <param name="rate">Occupancy rate</param>
    /// <response code="200">Request completed successfully</response>
    /// <response code="400">The rate is not valid</response>
    /// <response code="401">Authorization error</response>
    /// <response code="403">The user does not have enough permissions</response>
    /// <response code="500">Can't connect to the server</response>
    [HttpPost]
    [Authorize]
    public ObjectResult ReportOccupancy(uint id, float rate)
    {
        var whenReported = DateTime.Now;

        var token = Request.Headers[Constants.Authorization];
        var jwt = new JwtSecurityToken(token.ToString()[7..]);
        if (AccountAuthUtil.GetAccountType(jwt) != "POLIMI")
            return new UnauthorizedObjectResult(
                new JObject { { "error", "You don't have enough permissions" } }
            );

        if (rate < Constants.MinRate || rate > Constants.MaxRate)
            return new BadRequestObjectResult(
                new JObject
                {
                    {
                        "error",
                        "Rate must between " + Constants.MinRate + " and " + Constants.MaxRate
                    }
                }
            );

        const string q =
            "REPLACE INTO RoomOccupancyReports (room_id, user_id, rate, when_reported) VALUES (@id_room, sha2(@id_user, 256), @rate, @when_reported)";
        var count = DB.Execute(
            q,
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object?>
            {
                { "@id_room", id },
                { "@id_user", jwt.Subject },
                { "@rate", rate },
                { "@when_reported", whenReported }
            }
        );

        if (count <= 0)
            return StatusCode(500, new JObject { { "error", "Server error" } });

        return Ok("");
    }

    /// <summary>
    ///     Get the occupancy rate of a room
    /// </summary>
    /// <param name="id">Room ID</param>
    /// <response code="200">Request completed successfully</response>
    /// <response code="400">The room ID is not valid</response>
    /// <returns>The occupancy rate and the room ID</returns>
    [HttpGet]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public ObjectResult GetReportedOccupancy(uint id)
    {
        var result = GetReportedOccupancyJObject(id);
        if (result == null)
            return new BadRequestObjectResult(
                new JObject { { "error", "Can't get occupancy for room " + id } }
            );
        return Ok(result);
    }

    public static JObject? GetReportedOccupancyJObject(uint id)
    {
        const string q =
            "SELECT SUM(x.w * x.rate)/SUM(x.w) "
            + "FROM ("
            + "SELECT TIMESTAMPDIFF(SECOND, NOW(), when_reported) w, rate "
            + "FROM RoomOccupancyReports "
            + "WHERE room_id = @room_id AND when_reported >= @yesterday"
            + ") x ";
        var dict = new Dictionary<string, object?>
        {
            { "@room_id", id },
            { "@yesterday", DateTime.Now.AddDays(-1) }
        };
        var r = DB.ExecuteSelect(q, GlobalVariables.DbConfigVar, dict);
        if (r == null || r.Rows.Count == 0 || r.Rows[0].ItemArray.Length == 0)
            return null;

        var rate = DB.GetFirstValueFromDataTable(r);

        var jObject = new JObject { { "room_id", id }, { "occupancy_rate", new JValue(rate) } };
        return jObject;
    }
}
