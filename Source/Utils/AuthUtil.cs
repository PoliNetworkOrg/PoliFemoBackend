#region includes

using PoliFemoBackend.Source.Data;

#endregion

namespace PoliFemoBackend.Source.Utils;

public static class AuthUtil
{
    public static HttpResponseMessage? GetResponse(string code, string grant_type)
    {
        HttpClient httpClient = new();

        var clientSecret = GlobalVariables.secrets?["Azure"]?.ToString();
        if (clientSecret == null)
        {
            return null;
        }

        FormUrlEncodedContent formUrlEncodedContent = new(new Dictionary<string, string>
        {
            { "client_id", Constants.AzureClientId },
            { "client_secret", clientSecret},
            { grant_type == "authorization_code" ? "code" : "refresh_token", code},
            { "grant_type", grant_type }
        });

        return httpClient.PostAsync("https://login.microsoftonline.com/organizations/oauth2/v2.0/token", formUrlEncodedContent).Result;
    }
}