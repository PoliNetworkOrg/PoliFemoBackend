#region includes

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Utils;
using System.Diagnostics;

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
    public ObjectResult DeployLatest(string token)
    {
        try
        {
            if (token == JsonConvert.DeserializeObject<JObject>(System.IO.File.ReadAllText("secrets.json"))?["Deploy"]?.ToString())
            {
                Process.Start("./run.sh");
                Task.Run(() => {
                    Thread.Sleep(1000);
                    Environment.Exit(0);
                });
                return Ok("Request received successfully");
            }
            else
            {
                return Unauthorized("Invalid token");
            }
        }
        catch (Exception ex)
        {
            return ResultUtil.ExceptionResult(ex);
        }
    }
}