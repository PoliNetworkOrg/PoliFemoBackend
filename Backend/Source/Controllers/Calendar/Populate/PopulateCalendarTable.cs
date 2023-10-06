#region

using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Utils.Calendar.Populate;

#endregion

namespace PoliFemoBackend.Source.Controllers.Calendar.Populate;

[ApiController]
[ApiExplorerSettings(GroupName = "Calendar")]
[Route("/calendar")]
public class PopulateCalendarTable : ControllerBase
{
    /// <summary>
    ///     Adds date on Database
    /// </summary>
    /// <returns>Nothing</returns>
    /// <response code="200">Days Added</response>
    /// <response code="500">Can't connect to server or Days not Added</response>
    [HttpPut]
    public ObjectResult AddCalendarDb2(List<IFormFile> file, string year)
    {
        PopulateCalendarUtil.Populate(file, year);
        return Ok("OK");
    }
}
