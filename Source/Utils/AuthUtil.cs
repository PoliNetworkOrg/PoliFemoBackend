#region

using System.Data;
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

        var formContent = new Dictionary<string, string>
        {
            { "client_id", Constants.AzureClientId },
            { "scope", Constants.AzureScope },
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
                    formContent.Add("redirect_uri",
                        "https://francescolf-polinetworkorg-polifemobackend-7rvv557wfpjxp-5500.githubpreview.dev/index.html");
                    break;
                }

                default:
                {
                    formContent.Add("redirect_uri", "https://api.polinetwork.org/v1/auth/code");
                    break;
                }
            }

        var formUrlEncodedContent = new FormUrlEncodedContent(formContent);
        return httpClient.PostAsync("https://login.microsoftonline.com/organizations/oauth2/v2.0/token",
            formUrlEncodedContent).Result;
    }

    public static string? GetSubject(string token)
    {
        return GlobalVariables.TokenHandler?.ReadJwtToken(token.Split(" ")[1]).Subject;
    }


    public static bool HasPermission(string? userid, string permission)
    {
        var results = Database.Database.ExecuteSelect(
            "SELECT id_grant FROM permission, Grants, Users WHERE id_utente=sha2('@userid', 256) AND id_grant='@permission'",
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object?>
            {
                { "@userid", userid },
                { "@permission", permission }
            });
        return results != null;
    }

    public static bool HasGrantAndObjectPermission(string? userid, string permission, int objectid)
    {
        var results = Database.Database.ExecuteSelect(
            "SELECT id_grant FROM permission WHERE id_user=sha2('@userid', 256) AND id_grant='@permission' AND id_object=@objectid",
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object?>
            {
                { "@userid", userid },
                { "@permission", permission },
                { "@objectid", objectid }
            });
        return results != null;
    }

    public static string?[] GetPermissions(string? userid)
    {
        var results = Database.Database.ExecuteSelect(
            "SELECT DISTINCT name_grant FROM Grants, permission, Users WHERE name_grant=permission.id_grant AND permission.id_user=Users.id_utente AND id_utente=sha2('@userid', 256)",
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object?>
            {
                { "@userid", userid }
            });
        return results?.AsEnumerable().Select(x => x.Field<string>("name_grant")).ToArray() ?? Array.Empty<string>();
    }
}