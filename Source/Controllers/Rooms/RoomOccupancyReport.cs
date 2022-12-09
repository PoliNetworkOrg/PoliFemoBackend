using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

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
    public ObjectResult ReportOccupancy(string room)
    {
        //todo
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