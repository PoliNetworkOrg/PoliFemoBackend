#region

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PoliFemoBackend.Source.Utils;

#endregion

namespace PoliFemoBackend.Source.Controllers.Utils;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
[Route("[controller]")]
public class GetVersionsController : ControllerBase
{
    /// <summary>
    ///     Get the available versions of the API
    /// </summary>
    /// <returns></returns>
    [MapToApiVersion("1.0")]
    [HttpGet]
    [HttpPost]
    public ObjectResult GetVersions()
    {
        try
        {
            return Ok(JsonConvert.SerializeObject(new { versions = ApiVersionsManager.ReadApiVersions() },
                Formatting.Indented));
        }
        catch (Exception ex)
        {
            return ResultUtil.ExceptionResult(ex);
        }
    }
}