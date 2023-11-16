#region

using System.Net;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Enums;
using PoliFemoBackend.Source.Utils;
using PoliFemoBackend.Source.Utils.Auth;
using PoliFemoBackend.Source.Utils.Auth.CodeExchange;
using DB = PoliNetwork.Db.Utils.Database;

#endregion

namespace PoliFemoBackend.Source.Controllers.Auth;

[ApiController]
[ApiExplorerSettings(GroupName = "Auth")]
[Route("/auth/code")]
public class CodeExchangeController : ControllerBase
{
    /// <summary>
    ///     Get access and refresh token from Microsoft and perform user checks
    /// </summary>
    /// <remarks>
    ///     This is a callback endpoint. DO NOT call manually.
    /// </remarks>
    /// <param name="code">The authorization code</param>
    /// <param name="state">App ID</param>
    /// <response code="200">Request completed successfully</response>
    /// <response code="400">The code is not valid</response>
    /// <response code="403">The user is not using a valid org email</response>
    /// <returns>An access and a refresh token</returns>
    [HttpGet]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public ActionResult? CodeExchange(string code, int state)
    // https://login.microsoftonline.com/organizations/oauth2/v2.0/authorize?client_id=92602f24-dd8e-448e-a378-b1c575310f9d
    //      &scope=openid%20offline_access&response_type=code
    //      &login_hint=nome.cognome@mail.polimi.it&state=10010
    //      &redirect_uri=https://api.polinetwork.org/v1/auth/code
    {
        try
        {
            var response = AuthUtil.GetResponse(code, state, GrantType.AUTHORIZATION_CODE);

            if (response == null)
                return BadRequest("Client secret not found");

            var responseJson = JToken.Parse(response.Content.ReadAsStringAsync().Result);

            if (!response.IsSuccessStatusCode)
                return new BadRequestObjectResult(
                    new
                    {
                        error = "Error while exchanging code for token",
                        reason = responseJson.Value<string>("error")
                    }
                );

            var loginResultObject = LoginUtil.LoginUser(this, responseJson);
            if (loginResultObject is { ActionResult: not null })
                return loginResultObject.ActionResult;

            if (
                string.IsNullOrEmpty(loginResultObject?.Subject))
                return new BadRequestObjectResult(
                    new
                    {
                        error = "Error while exchanging code for token",
                        reason = "Subject or acctype is null or empty"
                    }
                );

            const string query =
                "INSERT IGNORE INTO Users VALUES(sha2(@subject, 256), @acctype, NOW(), 730);";
            var parameters = new Dictionary<string, object?>
            {
                { "@subject", loginResultObject.Subject },
                { "@acctype", loginResultObject.Acctype }
            };
            var results = DB.Execute(query, GlobalVariables.DbConfigVar, parameters);

            var responseObject = new JObject
            {
                { "access_token", responseJson["id_token"] },
                { "refresh_token", responseJson["refresh_token"] },
                { "expires_in", responseJson["expires_in"] }
            };
            return Ok(responseObject);
        }
        catch (MySqlException)
        {
            return new ObjectResult(
                new { error = "Database error", statusCode = HttpStatusCode.InternalServerError }
            );
        }
        catch (Exception ex)
        {
            return ResultUtil.ExceptionResult(ex);
        }
    }
}
