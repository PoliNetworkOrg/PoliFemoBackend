#region

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Enums;
using PoliFemoBackend.Source.Utils.Rooms;

#endregion

namespace PoliFemoBackend.Source.Controllers.Rooms;

[ApiController]
[ApiExplorerSettings(GroupName = "Rooms")]
[Route("/rooms/search")]
public class SearchRoomsController : ControllerBase
{
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
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> SearchRooms([BindRequired] string sede, DateTime? hourStart, DateTime? hourStop)

    {
        if (hourStart?.Hour < 8 || hourStop?.Hour > 20)
            return BadRequest(new JObject(new JProperty("error", "Invalid time range")));

        var (jArrayResults, doneEnum) = await SearchRoomUtil.SearchRooms(sede, hourStart, hourStop);
        switch (doneEnum)
        {
            case DoneEnum.DONE:
                return Ok(new JObject(new JProperty("free_rooms", jArrayResults)));
            case DoneEnum.SKIPPED:
                return NoContent();
            default:
            case DoneEnum.ERROR:
            {
                const string text4 = "Errore nella consultazione del sito del polimi!";
                return new ObjectResult(new { error = text4 })
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }
    }
}