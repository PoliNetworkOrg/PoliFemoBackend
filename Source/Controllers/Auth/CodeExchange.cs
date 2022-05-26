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
public class CodeExchangeController : ControllerBase
{
    /// <summary>
    ///     Gets access and refresh token from Microsoft and performs user checks.
    /// </summary>
    /// <remarks>
    ///     This is a callback endpoint. DO NOT call manually.
    /// </remarks>

    /// <param name="code">The authorization code</param>
    /// <response code="200">Returns the access token and refresh token</response>
    /// <response code="400">The code is not valid</response>  
    /// <response code="403">The user is not using a PoliMi email</response>  
    /// <returns>An access and a refresh token</returns>

    [MapToApiVersion("1.0")]
    [HttpGet]
    public ObjectResult CodeExchange(string code)
    // https://login.microsoftonline.com/organizations/oauth2/v2.0/authorize?client_id=92602f24-dd8e-448e-a378-b1c575310f9d&scope=openid%20offline_access&response_type=code&login_hint=nome.cognome@mail.polimi.it
    {
        HttpClient httpClient = new();
        FormUrlEncodedContent formUrlEncodedContent = new(new Dictionary<string, string>
        {
            { "client_id", Constants.Constants.AzureClientId },
            { "scope", "openid" },
            { "client_secret", Constants.Constants.AzureClientSecret },
            { "code", code },
            { "grant_type", "authorization_code" }
        });
        var response = httpClient.PostAsync($"https://login.microsoftonline.com/organizations/oauth2/v2.0/token", formUrlEncodedContent).Result;

        if (!response.IsSuccessStatusCode)
        {
            return new ObjectResult(new
            {
                error = "Error while exchanging code for token",
                statusCode = response.StatusCode
            });
        }

        var responseBody = response.Content.ReadAsStringAsync().Result;
        var responseJson = JToken.Parse(responseBody);

        var token = GlobalVariables.TokenHandler.ReadJwtToken(responseJson["access_token"]?.ToString());

        var domain = token.Payload["upn"].ToString();

        if (!domain.Contains("polimi.it"))
            return new ObjectResult(new
            {
                error = "Only PoliMi students are allowed",
                statusCode = HttpStatusCode.Forbidden
            });
        return Ok(responseBody);

    }
}