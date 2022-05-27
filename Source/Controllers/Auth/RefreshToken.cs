#region includes

using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Utils;

#endregion

namespace PoliFemoBackend.Source.Controllers.Auth;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
[Route("[controller]")]
public class RefreshTokenController : ControllerBase
{
    /// <summary>
    ///     Get a new access token from Microsoft.
    /// </summary>
    /// <param name="refreshToken">The refresh token</param>
    /// <response code="200">Returns the new access token</response>
    /// <response code="400">The refresh token is not valid</response>
    /// <returns>A new access token</returns>
    [MapToApiVersion("1.0")]
    [HttpGet]
    [HttpPost]
    public ObjectResult RefreshToken(string refreshToken)
    {
        try
        {
            HttpResponseMessage? response = AuthUtil.GetResponse(refreshToken, "refresh_token");

            if (response == null)
            {
                return BadRequest("Client secret not found");
            }

            if (!response.IsSuccessStatusCode)
            {
                return new ObjectResult(new
                {
                    error = "Refresh token not valid. Request a new authorization code.",
                    statusCode = response.StatusCode
                });
            }

            var responseBody = response.Content.ReadAsStringAsync().Result;
            return Ok(responseBody);
        }
        catch (Exception ex)
        {
            return ResultUtil.ExceptionResult(ex);
        }
    }
}