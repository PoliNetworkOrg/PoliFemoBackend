#region

using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Enums;

#endregion

namespace PoliFemoBackend.Source.Utils;

public static class AuthUtil
{
    public static HttpResponseMessage? GetResponse(string code, int state, GrantTypeEnum grantType)
    {
        HttpClient httpClient = new();

        var clientSecret = GlobalVariables.GetSecrets("Azure")?.ToString();
        if (clientSecret == null) return null;

        Dictionary<string, string> formcontent = new Dictionary<string, string>
        {
            { "client_id", Constants.AzureClientId },
            { "scope", Constants.AzureScope },
            { "client_secret", clientSecret },
            { grantType == GrantTypeEnum.authorization_code ? "code" : "refresh_token", code },
            { "grant_type", grantType.ToString() }
        };

        //non rimuovere - test
        if (grantType == GrantTypeEnum.authorization_code) {
            switch(state) {
                case 10020: {
                    formcontent.Add("redirect_uri", "https://francescolf-polinetworkorg-polifemobackend-7rvv557wfpjxp-5500.githubpreview.dev/index.html");
                    break;
                }
                
                default: {
                    formcontent.Add("redirect_uri", "https://api.polinetwork.org/v1/CodeExchange");
                    break;
                }
            }
        }

        FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(formcontent);
        return httpClient.PostAsync("https://login.microsoftonline.com/organizations/oauth2/v2.0/token", formUrlEncodedContent).Result;
    }

    /// <summary>
    ///     Get Sub id from auth token
    /// </summary>
    /// <param name="httpRequest">The http request from the user</param>
    /// <returns>The sub id of the auth token</returns>
    private static string? GetSubIdMicrosoftId(HttpRequest httpRequest)
    {
        var headers = httpRequest.Headers;
        return (from h in headers
            where h.Key == "Authorization"
            from h2 in h.Value
            select GlobalVariables.TokenHandler?.ReadJwtToken(h2)
            into token
            where token != null
            select token.Subject).FirstOrDefault();
    }

    /// <summary>
    ///     Detect if the user can insert articles
    /// </summary>
    /// <param name="httpRequest">The http request from the user</param>
    /// <returns>A bool, indicating if the user can insert articles</returns>
    public static bool CanInsertArticles(HttpRequest httpRequest)
    {
        var s = GetSubIdMicrosoftId(httpRequest);
        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (string.IsNullOrEmpty(s))
            return false;

        //todo query the db to see if the user can post articles
        return true;
    }
}