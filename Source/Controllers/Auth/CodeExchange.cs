#region

using System.IdentityModel.Tokens.Jwt;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Enums;
using PoliFemoBackend.Source.Utils;
using PoliFemoBackend.Source.Utils.Database;

#endregion

namespace PoliFemoBackend.Source.Controllers.Auth;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Auth")]
[Route("v{version:apiVersion}/auth/code")]
[Route("auth/code")]
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
    /// <response code="403">The user is not using a PoliMi org email</response>
    /// <returns>An access and a refresh token</returns>
    [MapToApiVersion("1.0")]
    [HttpGet]
    public ActionResult CodeExchange(string code, int state)
        // https://login.microsoftonline.com/organizations/oauth2/v2.0/authorize?client_id=92602f24-dd8e-448e-a378-b1c575310f9d
        //      &scope=openid%20offline_access&response_type=code
        //      &login_hint=nome.cognome@mail.polimi.it&state=10010
        //      &redirect_uri=https://api.polinetwork.org/v1/auth/code
    {
        try
        {
            var response = AuthUtil.GetResponse(code, state, GrantTypeEnum.authorization_code);

            if (response == null) return BadRequest("Client secret not found");

            var responseJson = JToken.Parse(response.Content.ReadAsStringAsync().Result);

            if (!response.IsSuccessStatusCode)
                return new BadRequestObjectResult(new
                {
                    error = "Error while exchanging code for token",
                    reason = responseJson.Value<string>("error")
                });

            string subject, acctype;
            JwtSecurityToken? token;


            try
            {
                token = GlobalVariables.TokenHandler?.ReadJwtToken(responseJson["access_token"]?.ToString());
                var domain = token?.Payload["upn"].ToString();
                if (domain == null || token?.Subject == null)
                    return new ObjectResult(new
                    {
                        error =
                            "The received code is not a valid organization code. Request a new authorization code and login again.",
                        statusCode = HttpStatusCode.BadRequest
                    });

                if (!domain.Contains("polimi.it"))
                    return new ForbidResult(
                        new JObject
                        {
                            { "error", "A PoliMi email is required" }
                        }.ToString()
                    );

                subject = token.Subject;
                acctype = "POLIMI";
            }
            catch (ArgumentException)
            {
                token = GlobalVariables.TokenHandler?.ReadJwtToken(responseJson["id_token"]?.ToString());
                subject = token?.Subject ??
                          throw new Exception(
                              "The received code is invalid. Request a new authorization code and login again.");
                acctype = "PERSONAL";
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(
                    new JObject
                    {
                        { "error", "The received code is invalid. Request a new authorization code and login again." },
                        { "reason", ex.Message }
                    }.ToString()
                );
            }

            var query = "INSERT IGNORE INTO Users VALUES(sha2(@subject, 256), @acctype, NOW());";
            var parameters = new Dictionary<string, object?>
            {
                { "@subject", subject },
                { "@acctype", acctype }
            };
            var results = Database.Execute(query, GlobalVariables.DbConfigVar, parameters);

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