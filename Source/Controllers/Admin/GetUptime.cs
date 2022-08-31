#region

using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils;

#endregion

namespace PoliFemoBackend.Source.Controllers.Admin;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Admin")]
[Route("v{version:apiVersion}/admin/uptime")]
[Route("/admin/uptime")]
public class GetUptimeController : ControllerBase
{
    /// <summary>
    ///     Returns the uptime of the backend server
    /// </summary>
    /// <returns></returns>
    [MapToApiVersion("1.0")]
    [HttpGet]
    public ObjectResult GetUptime()
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