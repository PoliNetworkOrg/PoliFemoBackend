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
    public ObjectResult ReportOccupancy(string room, float rate)
    {
        var whenReported = DateTime.Now;
        
        var user = AuthUtil.GetSubjectFromHttpRequest(this.Request);
        if (string.IsNullOrEmpty(user))
            return new ObjectResult(new JObject
            {
                { "error", "You don't have enough permissions" }
            });

        if (string.IsNullOrEmpty(room))
        {
            return new ObjectResult(new JObject
            {
                { "error", "Room can't be empty" }
            });
        }

        if (rate < Constants.MinRate || rate > Constants.MaxRate)
        {
            return new ObjectResult(new JObject
            {
                { "error", "Rate must between " + Constants.MinRate + " and " + Constants.MaxRate }
            });
        }
        
        //check dominio = polimi
        var domain = AuthUtil.GetDomainFromHttpRequest(this.Request);
        if (domain != "polimi.it")
            return new ObjectResult(new JObject
            {
                { "error", "You don't have enough permissions" }
            });

        var q = "INSERT INTO RoomOccupancyReport (id_room, id_user, rate, when_reported) VALUES (@id_room, @id_user, @rate, @when_reported)";
        var count = Database.Execute(q, DbConfig.DbConfigVar, new Dictionary<string, object?>()
        {
            {"@id_room", room},
            {"@id_user", user},
            {"@rate", rate},
            {"@when_reported", whenReported}
        });

        if (count <= 0)
        {
            return new ObjectResult(new JObject
            {
                { "error", "Report failed." }
            });
        }
        
        return Ok("");
    }
    
    [MapToApiVersion("1.0")]
    [HttpGet]
    [Authorize]
    public ObjectResult GetReportedOccupancy(string room)
    {
        //todo
        return Ok("");
    }
}