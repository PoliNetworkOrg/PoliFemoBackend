using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils;
using PoliFemoBackend.Source.Utils.Database;

namespace PoliFemoBackend.Source.Controllers.Rooms;

[ApiController]
[ApiExplorerSettings(GroupName = "Rooms")]
[Route("/rooms/{room}/occupancy")]

public class RoomOccupancyReport : ControllerBase
{
    /// <summary>
    ///     Send a report about the occupancy of a room
    /// </summary>
    /// <remarks>
    ///     The rate must be between 1 and 5
    /// </remarks>
    /// <param name="room">Room ID</param>
    /// <param name="rate">Occupancy rate</param>
    /// <response code="200">Request completed successfully</response>
    /// <response code="400">The rate is not valid</response>
    /// <response code="401">Authorization error</response>
    /// <response code="403">The user does not have enough permissions</response>
    /// <response code="500">Can't connect to the server</response>
    
    [HttpPost]
    [Authorize]
    public ObjectResult ReportOccupancy(uint room, float rate)
    {
        var whenReported = DateTime.Now;

        var token = Request.Headers[Constants.Authorization];
        var jwt = new JwtSecurityToken(token.ToString().Substring(7));
        if (AuthUtil.GetAccountType(jwt) != "POLIMI")
            return new UnauthorizedObjectResult(new JObject
            {
                { "error", "You don't have enough permissions" }
            });

        if (rate < Constants.MinRate || rate > Constants.MaxRate)
            return new BadRequestObjectResult(new JObject
            {
                { "error", "Rate must between " + Constants.MinRate + " and " + Constants.MaxRate }
            });

        var q =
            "REPLACE INTO RoomOccupancyReports (id_room, id_user, rate, when_reported) VALUES (@id_room, sha2(@id_user, 256), @rate, @when_reported)";
        var count = Database.Execute(q, DbConfig.DbConfigVar, new Dictionary<string, object?>
        {
            { "@id_room", room },
            { "@id_user", jwt.Subject },
            { "@rate", rate },
            { "@when_reported", whenReported }
        });

        if (count <= 0)
            return StatusCode(500, new JObject
            {
                { "error", "Server error" }
            });

        return Ok("");
    }


    /// <summary>
    ///     Get the occupancy rate of a room
    /// </summary>
    /// <param name="room">Room ID</param>
    /// <response code="200">Request completed successfully</response>
    /// <response code="400">The room ID is not valid</response>
    /// <returns>The occupancy rate and the room ID</returns>
    
    [HttpGet]
    public ObjectResult GetReportedOccupancy(uint room)
    {
        const string q = "SELECT SUM(x.w * x.rate)/SUM(x.w) " +
                         "FROM (" +
                         "SELECT TIMESTAMPDIFF(SECOND, NOW(), when_reported) w, rate " +
                         "FROM RoomOccupancyReports " +
                         "WHERE id_room = @id_room AND when_reported >= @yesterday" +
                         ") x ";
        var dict = new Dictionary<string, object?>
        {
            { "@id_room", room },
            { "@yesterday", DateTime.Now.AddDays(-1) }
        };
        var r = Database.ExecuteSelect(q, DbConfig.DbConfigVar, dict);
        if (r == null || r.Rows.Count == 0 || r.Rows[0].ItemArray.Length == 0)
            return new BadRequestObjectResult(new JObject
            {
                { "error", "Can't get occupancy for room " + room }
            });

        var rate = Database.GetFirstValueFromDataTable(r);

        return Ok(new JObject
        {
            { "room_id", room },
            { "occupancy_rate", new JValue(rate) }
        });
    }
}