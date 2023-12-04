#region

using System.Net.Http.Headers;
using AspNetCore.Proxy;
using AspNetCore.Proxy.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PoliFemoBackend.Source.Data;

#endregion

namespace PoliFemoBackend.Source.Controllers.Admin;

[ApiController]
[ApiExplorerSettings(GroupName = "Updates")]
[Route("/updates/assets/{name}")]
public class GetUpdateAssetController : ControllerBase
{
    private readonly HttpProxyOptions _httpOptions = HttpProxyOptionsBuilder
        .Instance
        .WithAfterReceive(
            (c, hrm) =>
            {
                MediaTypeHeaderValue header;
                var ext = c.Request.Path.Value?.Split(".")[1];
                switch (ext)
                {
                    case "svg":
                        header = new MediaTypeHeaderValue("image/svg+xml");
                        break;
                    case "png":
                        header = new MediaTypeHeaderValue("image/png");
                        break;
                    case "ttf":
                        header = new MediaTypeHeaderValue("font/ttf");
                        break;
                    default:
                        header = new MediaTypeHeaderValue("application/octet-stream");
                        break;
                }

                hrm.Content.Headers.ContentType = header;

                return Task.CompletedTask;
            }
        )
        .WithHandleFailure(
            async (c, e) =>
            {
                c.Response.StatusCode = 502;
                await c.Response.WriteAsync("Error while contacting GitHub");
            }
        )
        .Build();

    /// <summary>
    ///     Get an update asset
    /// </summary>
    /// <param name="name">The name of the asset</param>
    [HttpGet]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public Task GetAsset([BindRequired] string name)
    {
        if (name.Split(".").Length != 2)
            return BadRequest()
                .ExecuteResultAsync(
                    new ActionContext(HttpContext, new RouteData(), new ActionDescriptor())
                );

        var ext = name.Split(".")[1];
        return this.HttpProxyAsync(Constants.AssetsUrl + name.Split(".")[0], _httpOptions);
    }
}
