#region

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Utils.Rooms.Search;

#endregion

namespace PoliFemoBackend.Source.Controllers.Rooms.Search;

[ApiController]
[ApiExplorerSettings(GroupName = "Rooms")]
[Route("/rooms/search_hour")]
public class SearchRoomsWithHourController : ControllerBase
{
    private const int SecondsToCacheSearch = 60 * 60; //an hour

    /// <summary>
    ///     Search for available rooms in a given time range
    /// </summary>
    /// <param name="sede" example="MIA">Possible values: MIA, MIB, LCF, MNI, PCL</param>
    /// <param name="hourStart" example="2022-05-18T12:15:00Z">Start time</param>
    /// <param name="hourStop" example="2022-05-18T14:15:00Z">End time</param>
    /// <returns>A JSON array of free rooms</returns>
    /// <response code="200">Request completed successfully</response>
    /// <response code="204">No available rooms</response>
    /// <response code="400">Invalid time range (outside 8-20)</response>
    /// <response code="500">Can't connect to poli servers</response>
    [HttpGet]
    [ResponseCache(VaryByQueryKeys = new[] { "*" }, Duration = SecondsToCacheSearch)]
    public async Task<IActionResult> SearchRooms([BindRequired] string sede, DateTime? hourStart, DateTime? hourStop)
    {
        return hourStart?.Hour < 8 || hourStop?.Hour > 20
            ? BadRequest(new JObject(new JProperty("error", "Invalid time range")))
            : await SearchRoomUtil.ReturnSearchResults(sede, hourStart, hourStop, this);
    }
}