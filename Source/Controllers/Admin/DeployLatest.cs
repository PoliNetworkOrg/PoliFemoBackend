#region

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils;

#endregion

namespace PoliFemoBackend.Source.Controllers.Admin;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Admin")]
[Route("v{version:apiVersion}/admin/deploy")]
[Route("/admin/deploy")]
public class DeployLatestController : ControllerBase
{
    /// <summary>
    ///     Shuts the server down and reboots it on the latest release available on GitHub
    /// </summary>
    /// <returns></returns>
    [MapToApiVersion("1.0")]
    [HttpGet]
    public ObjectResult DeployLatest(string token, bool dev = false)
    {
        try
        {
            if (token != GlobalVariables.GetSecrets("Deploy")?.ToString())
                return Unauthorized("Invalid token");


            Task.Run(() =>
            {
                GracefullyShutdown.Shutdown();
                try
                {
                    var processStartInfo = new ProcessStartInfo
                    {
                        FileName = "/usr/bin/sudo", Arguments = "service polifemo restart", UseShellExecute = false,
                        RedirectStandardOutput = true, CreateNoWindow = true
                    };

                    if (dev)
                    {
                        processStartInfo.Arguments = "service polifemodev restart";
                    }
                    var process = new Process { StartInfo = processStartInfo };
                    process.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
            return Ok("Request received successfully");
        }
        catch (Exception ex)
        {
            return ResultUtil.ExceptionResult(ex);
        }
    }
}