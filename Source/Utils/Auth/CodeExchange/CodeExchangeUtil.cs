using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Controllers.Auth;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Enums;

namespace PoliFemoBackend.Source.Utils.Auth.CodeExchange;

public static class CodeExchangeUtil
{
    /// <summary>
    ///     Get access and refresh token from Microsoft and perform user checks
    /// </summary>
    /// <remarks>
    ///     This is a callback endpoint. DO NOT call manually.
    /// </remarks>
    /// <param name="code">The authorization code</param>
    /// <param name="state">App ID</param>
    /// <param name="codeExchangeController">instance of the CodeExchangeController</param>
    /// <response code="200">Request completed successfully</response>
    /// <response code="400">The code is not valid</response>
    /// <response code="403">The user is not using a valid org email</response>
    /// <returns>An access and a refresh token</returns>
    internal static ActionResult? CodeExchangeMethod(string code, int state,
        CodeExchangeController codeExchangeController)
    {
        var response = AuthUtil.GetResponse(code, state, GrantTypeEnum.authorization_code);

        if (response == null) return codeExchangeController.BadRequest("Client secret not found");

        var responseJson = JToken.Parse(response.Content.ReadAsStringAsync().Result);

        if (!response.IsSuccessStatusCode)
            return new BadRequestObjectResult(new
            {
                error = "Error while exchanging code for token",
                reason = responseJson.Value<string>("error")
            });

        var loginResultObject = LoginUtil.LoginUser(codeExchangeController, responseJson);
        if (loginResultObject is { ActionResult: { } })
            return loginResultObject.ActionResult;

        AddUserToDb(loginResultObject?.Subject, loginResultObject?.Acctype);

        var responseObject = new JObject
        {
            { "access_token", responseJson["id_token"] },
            { "refresh_token", responseJson["refresh_token"] },
            { "expires_in", responseJson["expires_in"] }
        };
        return codeExchangeController.Ok(responseObject);
    }


    private static void AddUserToDb(string? subject, string? acctype)
    {
        if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(acctype))
            return;

        const string query = "INSERT IGNORE INTO Users VALUES(sha2(@subject, 256), @acctype, NOW(), 730);";
        var parameters = new Dictionary<string, object?>
        {
            { "@subject", subject },
            { "@acctype", acctype }
        };
        var results = Database.Database.Execute(query, GlobalVariables.DbConfigVar, parameters);
    }
}