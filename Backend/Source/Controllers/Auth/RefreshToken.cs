#region

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Enums;
using PoliFemoBackend.Source.Utils;
using PoliFemoBackend.Source.Utils.Auth;

#endregion

namespace PoliFemoBackend.Source.Controllers.Auth;

[ApiController]
[ApiExplorerSettings(GroupName = "Auth")]
[Route("/auth/refresh")]
public class RefreshTokenController : ControllerBase
{
    /// <summary>
    ///     Get a new access token from Microsoft
    /// </summary>
    /// <remarks>
    ///     The refresh token is sent in the "Token" header
    /// </remarks>
    /// <response code="200">Request completed successfully</response>
    /// <response code="400">The refresh token is not valid</response>
    /// <returns>A new access token</returns>
    [HttpGet]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public ObjectResult? RefreshToken()
    {
        try
        {
            var refreshToken = Request.Headers["Token"].ToString();
            var response = AuthUtil.GetResponse(refreshToken, 99999, GrantType.REFRESH_TOKEN);

            if (response == null)
                return BadRequest("Client secret not found");

            if (!response.IsSuccessStatusCode)
                return new ObjectResult(
                    new
                    {
                        error = "Refresh token not valid. Request a new authorization code.",
                        statusCode = response.StatusCode
                    }
                );

            var responseBody = response.Content.ReadAsStringAsync().Result;
            Response.ContentType = "application/json";

            var responseJson = JObject.Parse(responseBody);
            var resultJson = new JObject
            {
                ["access_token"] = responseJson["id_token"],
                ["refresh_token"] = responseJson["refresh_token"],
                ["expires_in"] = responseJson["expires_in"]
            };

            return new ObjectResult(resultJson);
        }
        catch (Exception ex)
        {
            return ResultUtil.ExceptionResult(ex);
        }
    }
}
