#region includes

using PoliFemoBackend.Source.Data;

#endregion

namespace PoliFemoBackend.Source.Utils;

public static class AuthUtil
{
    public static HttpResponseMessage? GetResponse(string code)
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
            { "refresh_token", code},
            { "grant_type", "refresh_token" }
        });

        return httpClient.PostAsync("https://login.microsoftonline.com/organizations/oauth2/v2.0/token", formUrlEncodedContent).Result;
    }
}