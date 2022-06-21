#region

using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Enums;

#endregion

namespace PoliFemoBackend.Source.Utils;

public static class AuthUtil
{
    public static HttpResponseMessage? GetResponse(string code, GrantTypeEnum grantType)
    {
        HttpClient httpClient = new();

        var clientSecret = GlobalVariables.GetSecrets("Azure")?.ToString();
        if (clientSecret == null) return null;

        FormUrlEncodedContent formUrlEncodedContent = new(new Dictionary<string, string>
        {
            { "client_id", Constants.AzureClientId },
            { "scope", Constants.AzureScope },
            { "client_secret", clientSecret },
            { grantType == GrantTypeEnum.authorization_code ? "code" : "refresh_token", code },
            { "grant_type", grantType.ToString() }
        });

        return httpClient.PostAsync("https://login.microsoftonline.com/organizations/oauth2/v2.0/token",
            formUrlEncodedContent).Result;
    }
}