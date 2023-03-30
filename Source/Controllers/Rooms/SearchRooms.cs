#region

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Enums;
using PoliFemoBackend.Source.Utils.Rooms.Search;

#endregion

namespace PoliFemoBackend.Source.Controllers.Rooms.Search;

[ApiController]
[ApiExplorerSettings(GroupName = "Rooms")]
[Route("/rooms/search")]
public class SearchRoomsController : ControllerBase
{
    private const int SecondsToCacheSearch = 60 * 60; //an hour

    /// <summary>
    ///     Search for available rooms in a given date, from 8:00 to 20:00
    /// </summary>
    /// <param name="date">Date in format yyyy-mm-dd</param>
    /// <returns>A JSON array of free rooms</returns>
    /// <response code="200">Request completed successfully</response>
    /// <response code="204">No available rooms</response>
    /// <response code="500">Can't connect to poli servers</response>
    [HttpGet]
    [ResponseCache(VaryByQueryKeys = new[] { "*" }, Duration = SecondsToCacheSearch)]
    public async Task<IActionResult> SearchRooms([BindRequired] string date)
    {
        var sedi = new List<string> { "MIA", "MIB", "LCF", "MNI", "PCL" };
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

        var jObject = new List<JObject>();
        var doneEnums = new List<DoneEnum>();
        foreach (var sede in sedi)
        {
            var (jArrayResults, doneEnum) = await SearchRoomUtil.SearchRooms(sede, hourStart, hourStop);
            jObject.Add(new JObject { { sede, jArrayResults } });
            doneEnums.Add(doneEnum);
        }

        return doneEnums.Contains(DoneEnum.ERROR)
            ? SearchRoomUtil.ReturnActionResult(this, DoneEnum.ERROR, null)
            : SearchRoomUtil.ReturnActionResult(this, jObject.Count == 0
                ? DoneEnum.SKIPPED
                : DoneEnum.DONE, jObject);
    }
}