using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils;
using PoliFemoBackend.Source.Utils.Database;

namespace PoliFemoBackend.Source.Controllers.Rooms;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Rooms")]
[Route("v{version:apiVersion}/rooms/{room}/occupancy")]
[Route("/rooms/{room}/occupancy")]
public class RoomOccupancyReport : ControllerBase
{
    [MapToApiVersion("1.0")]
    [HttpPost]
    [Authorize]
    public ObjectResult ReportOccupancy(uint room, float rate)
    {
        var whenReported = DateTime.Now;

        var user = AuthUtil.GetSubjectFromHttpRequest(Request);
        if (string.IsNullOrEmpty(user))
            return new ObjectResult(new JObject
            {
                { "error", "You don't have enough permissions" }
            });

        if (rate < Constants.MinRate || rate > Constants.MaxRate)
            return new ObjectResult(new JObject
            {
                { "error", "Rate must between " + Constants.MinRate + " and " + Constants.MaxRate }
            });

        //check dominio = polimi
        var domain = AuthUtil.GetDomainFromHttpRequest(Request);
        if (domain != "polimi.it")
            return new ObjectResult(new JObject
            {
                { "error", "You don't have enough permissions" }
            });

        var q =
            "REPLACE INTO RoomOccupancyReport (id_room, id_user, rate, when_reported) VALUES (@id_room, @id_user, @rate, @when_reported)";
        var count = Database.Execute(q, DbConfig.DbConfigVar, new Dictionary<string, object?>
        {
            { "@id_room", room },
            { "@id_user", user },
            { "@rate", rate },
            { "@when_reported", whenReported }
        });

        if (count <= 0)
            return new ObjectResult(new JObject
            {
                { "error", "Report failed." }
            });

        return Ok("");
    }

    [MapToApiVersion("1.0")]
    [HttpGet]
    public ObjectResult GetReportedOccupancy(uint room)
    {
        const string q = "SELECT SUM(x.w * x.rate)/SUM(x.w) " +
                         "FROM (" +
                         "SELECT TIMESTAMPDIFF(SECOND, NOW(), when_reported) w, rate " +
                         "FROM RoomOccupancyReport " +
                         "WHERE id_room = @room AND when_reported >= @yesterday" +
                         ") x ";
        var dict = new Dictionary<string, object?>
        {
            { "@id_room", room },
            { "@yesterday", DateTime.Now.AddDays(-1) }
        };
        var r = Database.ExecuteSelect(q, DbConfig.DbConfigVar, dict);
        if (r == null || r.Rows.Count == 0 || r.Rows[0].ItemArray.Length == 0)
            return new ObjectResult(new JObject
            {
                { "error", "Can't get occupancy for room " + room }
            });

        var rate = Database.GetFirstValueFromDataTable(r);

        return Ok(new JObject
        {
            { "room", room },
            { "rate", new JValue(rate) }
        });
    }
}