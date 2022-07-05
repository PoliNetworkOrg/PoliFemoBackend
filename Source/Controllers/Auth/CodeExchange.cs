#region

using System.Net;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Enums;
using PoliFemoBackend.Source.Utils;

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
        try
        {
            var response = AuthUtil.GetResponse(code, GrantTypeEnum.authorization_code);

            if (response == null) return BadRequest("Client secret not found");

            var responseJson = JToken.Parse(response.Content.ReadAsStringAsync().Result);

            if (!response.IsSuccessStatusCode)
                return new ObjectResult(new
                {
                    error = "Error while exchanging code for token",
                    reason = responseJson.Value<string>("error"),
                    statusCode = response.StatusCode
                });

            var token = GlobalVariables.TokenHandler?.ReadJwtToken(responseJson["access_token"]?.ToString());
            var domain = token?.Payload["upn"].ToString();
            if (domain == null || token?.Subject == null)
                return new ObjectResult(new
                {
                    error =
                        "The received code is not a valid organization code. Request a new authorization code and login with your PoliMi account",
                    statusCode = HttpStatusCode.BadRequest
                });

            if (!domain.Contains("polimi.it"))
                return new ObjectResult(new
                {
                    error = "Only PoliMi students are allowed",
                    statusCode = HttpStatusCode.Forbidden
                });

            var query = "insert ignore into utente values(sha2('" + token.Subject + "', 256), 1)";
            var results = Database.Execute(query, GlobalVariables.DbConfigVar);
            return Ok(responseJson);
        }
        catch (MySqlException)
        {
            return new ObjectResult(new
                {
                    error = "Database error",
                    statusCode = HttpStatusCode.InternalServerError
                });
        }
        catch (Exception ex)
        {
            return ResultUtil.ExceptionResult(ex);
        }
    }
}