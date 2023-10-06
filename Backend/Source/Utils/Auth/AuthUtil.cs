#region

using System.IdentityModel.Tokens.Jwt;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Enums;

#endregion

namespace PoliFemoBackend.Source.Utils.Auth;

public static class AuthUtil
{
    public static HttpResponseMessage? GetResponse(string code, int state, GrantTypeEnum grantType)
    {
        HttpClient httpClient = new();

        var clientSecret = GlobalVariables.GetSecrets("Azure")?.ToString();
        if (clientSecret == null)
            return null;

        var formContent = new Dictionary<string, string>
        {
            { "client_id", Constants.AzureClientId },
            { "client_secret", clientSecret },
            { grantType == GrantTypeEnum.authorization_code ? "code" : "refresh_token", code },
            { "grant_type", grantType.ToString() }
        };

        //non rimuovere - test
        if (grantType == GrantTypeEnum.authorization_code)
            switch (state)
            {
                case 10020:
                {
                    formContent.Add("redirect_uri", "https://dashboard.polinetwork.org");
                    break;
                }

                default:
                {
                    formContent.Add(
                        "redirect_uri",
                        "https://api.polinetwork.org" + GlobalVariables.BasePath + "auth/code"
                    );
                    break;
                }
            }

        var formUrlEncodedContent = new FormUrlEncodedContent(formContent);
        return httpClient
            .PostAsync(
                "https://login.microsoftonline.com/common/oauth2/v2.0/token",
                formUrlEncodedContent
            )
            .Result;
    }

    /// <summary>
    ///     Get user/subject from HttpRequest
    /// </summary>
    /// <param name="httpRequest">HttpRequest containing the token</param>
    /// <returns>Subject/User</returns>
    public static string? GetSubjectFromHttpRequest(HttpRequest httpRequest)
    {
        var token = httpRequest.Headers[Constants.Authorization];
        return GetSubjectFromToken(token);
    }

    private static string? GetSubjectFromToken(string? token)
    {
        var jwtSecurityToken = GetJwtSecurityTokenFromStringToken(token);
        return jwtSecurityToken?.Subject;
    }

    private static JwtSecurityToken? GetJwtSecurityTokenFromStringToken(string? token)
    {
        if (string.IsNullOrEmpty(token))
            return null;

        var strings = token.Split(" ");
        if (strings.Length < 2)
            return null;

        var s = strings[1];
        var jwtSecurityToken = GlobalVariables.TokenHandler?.ReadJwtToken(s);
        return jwtSecurityToken;
    }
}
