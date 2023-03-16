#region

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PoliFemoBackend.Source.Utils.Rooms.Search;

#endregion

namespace PoliFemoBackend.Source.Controllers.Rooms.Search;

[ApiController]
[ApiExplorerSettings(GroupName = "Rooms")]
[Route("/rooms/search_day")]
public class SearchRoomsWithDayController : ControllerBase
{
    private const int SecondsToCacheSearch = 60 * 60; //an hour

    /// <summary>
    ///     Search for available rooms in a given date, from 8:00 to 20:00
    /// </summary>
    /// <param name="sede" example="MIA">Possible values: MIA, MIB, LCF, MNI, PCL</param>
    /// <param name="date">Date in format yyyy-mm-dd</param>
    /// <returns>A JSON array of free rooms</returns>
    /// <response code="200">Request completed successfully</response>
    /// <response code="204">No available rooms</response>
    /// <response code="500">Can't connect to poli servers</response>
    [HttpGet]
    [ResponseCache(VaryByQueryKeys = new[] { "*" }, Duration = SecondsToCacheSearch)]
    public async Task<IActionResult> SearchRooms([BindRequired] string sede, [BindRequired] string date)
    {
        DateOnly dateOnly;
        try
        {
            dateOnly = DateOnly.Parse(date);
        }
        catch (Exception)
        {
            return BadRequest(new { error = "Invalid date format" });
        }

        var hourStart = new DateTime(dateOnly.Year, dateOnly.Month, dateOnly.Day, 8, 0, 0);
        var hourStop = new DateTime(dateOnly.Year, dateOnly.Month, dateOnly.Day, 20, 0, 0);

        return await SearchRoomUtil.ReturnSearchResults(sede, hourStart, hourStop, this);
    }
}