#region

using System.IdentityModel.Tokens.Jwt;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Enums;
using PoliFemoBackend.Source.Objects.Permission;

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
        return httpClient.PostAsync("https://login.microsoftonline.com/common/oauth2/v2.0/token",
            formUrlEncodedContent).Result;
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

    private static string? GetSubjectFromToken(string token)
    {
        var jwtSecurityToken = GetJwtSecurityTokenFromStringToken(token);
        return jwtSecurityToken?.Subject;
    }

    private static JwtSecurityToken? GetJwtSecurityTokenFromStringToken(string token)
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


    public static bool HasPermission(string? userid, string permission)
    {
        var results = Database.Database.ExecuteSelect(
            "SELECT grant_id FROM permission, Grants, Users WHERE id_user=sha2(@userid, 256) AND id_grant=@permission",
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
            "SELECT id_grant FROM permission WHERE id_user=sha2(@userid, 256) AND id_grant=@permission AND id_object=@objectid",
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object?>
            {
                { "@userid", userid },
                { "@permission", permission },
                { "@objectid", objectid }
            });
        return results != null;
    }

    public static List<PermissionGrantObject> GetPermissions(string? userid, bool convert = true)
    {
        var query =
            "SELECT DISTINCT name_grant, id_object FROM Grants, permission, Users WHERE name_grant=permission.id_grant AND permission.id_user=Users.id_user ";
        if (convert) query += "AND id_user=sha2(@userid, 256)";
        else query += "AND id_user=@userid";

        var results = Database.Database.ExecuteSelect(
            query,
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object?>
            {
                { "@userid", userid }
            });
        var array = new List<PermissionGrantObject>();
        for (var i = 0; i < results?.Rows.Count; i++)
            array.Add(
                new PermissionGrantObject(
                    results.Rows[i]["name_grant"].ToString(),
                    results.Rows[i]["id_object"].ToString()
                )
            );
        return array;
    }

    public static string?[] GetAuthorizedAuthors(string? userid)
    {
        var results = Database.Database.ExecuteSelect(
            "SELECT a.* FROM Authors a, permission p WHERE p.id_user = sha2(@userid, 256) AND a.id_author = p.id_object AND p.id_grant = 'authors'",
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object?>
            {
                { "@userid", userid }
            });
        var array = new string?[results?.Rows.Count ?? 0];
        for (var i = 0; i < results?.Rows.Count; i++) array[i] = results.Rows[i]["name_"].ToString();
        return array;
    }

    public static string GetAccountType(JwtSecurityToken jwtSecurityToken)
    {
        var results = Database.Database.ExecuteSelect(
            "SELECT account_type FROM Users WHERE id_user = sha2(@userid, 256)",
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object?>
            {
                { "@userid", jwtSecurityToken.Subject }
            });
        return results?.Rows[0]["account_type"].ToString() ?? "NONE";
    }
}