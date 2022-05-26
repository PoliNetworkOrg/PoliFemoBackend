#region includes

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliNetworkBot_CSharp.Code.Data;

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
    /// <param name="refresh_token">The refresh token</param>
    /// <response code="200">Returns the new access token</response>
    /// <response code="400">The refresh token is not valid</response>
    /// <returns>A new access token</returns>
    [MapToApiVersion("1.0")]
    [HttpGet]
    [HttpPost]
    public ObjectResult RefreshToken(string refresh_token)
    {
        HttpClient httpClient = new();
        FormUrlEncodedContent formUrlEncodedContent = new(new Dictionary<string, string>
        {
            { "client_id", Constants.Constants.AzureClientId },
            { "client_secret", Constants.Constants.AzureClientSecret },
            { "refresh_token", refresh_token},
            { "grant_type", "refresh_token" }
        });
        var response = httpClient.PostAsync($"https://login.microsoftonline.com/organizations/oauth2/v2.0/token", formUrlEncodedContent).Result;

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
}