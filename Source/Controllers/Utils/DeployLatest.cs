#region includes

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PoliFemoBackend.Source.Utils;

#endregion

namespace PoliFemoBackend.Source.Controllers.Utils;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
[Route("[controller]")]
public class DeployLatestController : ControllerBase
{
    /// <summary>
    ///     Shuts the server down and reboots it on the latest release available on GitHub
    /// </summary>
    /// <returns></returns>
    [MapToApiVersion("1.0")]
    [HttpGet]
    [HttpPost]
    public ObjectResult DeployLatest()
    {
        try
        {
            return Ok(JsonConvert.SerializeObject(new { versions = ApiVersionsManager.ReadApiVersions() }, Formatting.Indented));
        }
        catch (Exception ex)
        {
            return ResultUtil.ExceptionResult(ex);
        }
    }
}