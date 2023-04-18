using System.IdentityModel.Tokens.Jwt;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Objects.Auth;

namespace PoliFemoBackend.Source.Utils.Auth.CodeExchange;

public static class LoginUtil
{
    internal static LoginResultObject? LoginUser(ControllerBase codeExchangeController, JToken responseJson)
    {
        string? acctype = null;
        string? subject = null;
        ActionResult? actionResult = null;

        try
        {
            if (Login2(codeExchangeController, responseJson, ref acctype, ref subject, out var loginUser)) return loginUser;
        }
        catch (ArgumentException)
        {
            var token = GlobalVariables.TokenHandler?.ReadJwtToken(responseJson["id_token"]?.ToString());
            subject = token?.Subject ??
                      throw new Exception(
                          "The received code is invalid. Request a new authorization code and login again.");
            acctype = "PERSONAL";
        }
        catch (Exception ex)
        {
            {
                actionResult = new BadRequestObjectResult(
                    new JObject
                    {
                        { "error", "The received code is invalid. Request a new authorization code and login again." },
                        { "reason", ex.Message }
                    }.ToString()
                );
                return new LoginResultObject(acctype, subject, actionResult);
            }
        }

        return new LoginResultObject(acctype, subject, actionResult);
    }

    private static bool Login2(ControllerBase codeExchangeController, JToken responseJson, ref string? acctype,
        ref string? subject, out LoginResultObject? loginUser)
    {
        ActionResult? actionResult;
        var token = GlobalVariables.TokenHandler?.ReadJwtToken(responseJson["access_token"]?.ToString());
        var domain = token?.Payload["upn"].ToString()?.Split('@')[1];
        if (domain == null || token?.Subject == null)
        {
            actionResult = new BadRequestObjectResult(new
            {
                error =
                    "The received code is not a valid organization code. Request a new authorization code and login again.",
                statusCode = HttpStatusCode.BadRequest
            });
            {
                loginUser = new LoginResultObject(acctype, subject, actionResult);
                return true;
            }
        }

        switch (domain)
        {
            case "polimi.it":
            case "mail.polimi.it":
                acctype = "POLIMI";
                break;
            case "polinetwork.org":
                acctype = "POLINETWORK";
                break;
            default:
            {
                actionResult = codeExchangeController.StatusCode(403, new
                {
                    error = "The user is not using a valid org email. Please use a public account."
                });
                {
                    loginUser = new LoginResultObject(acctype, subject, actionResult);
                    return true;
                }
            }
        }

        token = GlobalVariables.TokenHandler?.ReadJwtToken(responseJson["id_token"]?.ToString());
        subject = token != null ? token.Subject : throw new Exception("Token is null");
        loginUser = null;
        return false;
    }
}