#region

using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils;

#endregion

namespace PoliFemoBackend.Source.Controllers.Admin;

[ApiController]
[ApiExplorerSettings(GroupName = "Admin")]
[Route("/admin/uptime")]
public class GetUptimeController : ControllerBase
{
    /// <summary>
    ///     Get the uptime of the backend server
    /// </summary>
    /// <returns>The number of seconds of uptime</returns>
    [HttpGet]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public ObjectResult? GetUptime()
    {
        try
        {
            return Ok((DateTime.Now - GlobalVariables.Start).Ticks / 10000000);
        }
        catch (Exception ex)
        {
            return ResultUtil.ExceptionResult(ex);
        }
    }
}