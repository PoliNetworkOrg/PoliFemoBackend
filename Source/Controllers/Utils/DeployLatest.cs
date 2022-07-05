#region

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Data;
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
    public ObjectResult DeployLatest(string token)
    {
        try
        {
            if (token != GlobalVariables.GetSecrets("Deploy")?.ToString())
                return Unauthorized("Invalid token");


            Task.Run(() =>
            {
                GracefullyShutdown.Shutdown();
                Process.Start("sudo screen ./run.sh");
                Thread.Sleep(1000);
                Environment.Exit(0);
            });
            return Ok("Request received successfully");
        }
        catch (Exception ex)
        {
            return ResultUtil.ExceptionResult(ex);
        }
    }
}