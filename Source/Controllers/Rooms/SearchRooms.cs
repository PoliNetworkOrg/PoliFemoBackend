#region

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PoliFemoBackend.Source.Utils;

#endregion

namespace PoliFemoBackend.Source.Controllers.Rooms;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
[Route("[controller]")]
public class SearchRoomsController : ControllerBase
{
    /// <summary>
    ///     Searches for available rooms in a given time range
    /// </summary>
    /// <param name="sede" example="MIA">Possible values: MIA, MIB</param>
    /// <param name="hourStart" example="2022-05-18T12:15:00Z">Start time</param>
    /// <param name="hourStop" example="2022-05-18T14:15:00Z">End time</param>
    /// <returns>An array of free rooms</returns>
    /// <response code="200">Returns the array of free rooms</response>
    /// <response code="500">Can't connect to poli servers</response>
    /// <response code="204">No available rooms</response>
    [MapToApiVersion("1.0")]
    [HttpGet]
    [HttpPost]
    public async Task<IActionResult> SearchRooms([BindRequired] string sede, [BindRequired] DateTime hourStart,
        [BindRequired] DateTime hourStop)
    {
        hourStop = hourStop.AddMinutes(-1);
        var t3 = await RoomUtil.GetDailySituationOnDate(hourStart, sede);
        if (t3 is null || t3.Count == 0)
        {
            const string text4 = "Errore nella consultazione del sito del polimi!";
            return new ObjectResult(new { error = text4 })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }

        var t4 = RoomUtil.GetFreeRooms(t3[0], hourStart, hourStop);
        if (t4 is null || t4.Count == 0) return NoContent();

        var json = new { freeRooms = t4 };


        return Ok(json);
    }
}