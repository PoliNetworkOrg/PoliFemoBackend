#region

using System.IdentityModel.Tokens.Jwt;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Enums;
using PoliFemoBackend.Source.Objects.Permissions;

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
                        "https://dashboard.polinetwork.org");
                    break;
                }

                default:
                {
                    formContent.Add("redirect_uri", "https://api.polinetwork.org" + GlobalVariables.BasePath + "auth/code");
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
            "SELECT grant_id FROM permissions, Grants, Users WHERE Users.user_id=sha2(@userid, 256) AND grant_id=@permission",
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
            "SELECT grant_id FROM permissions WHERE user_id=sha2(@userid, 256) AND grant_id=@permission AND object_id=@objectid",
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object?>
            {
                { "@userid", userid },
                { "@permission", permission },
                { "@objectid", objectid }
            });
        return results != null;
    }

    public static List<Grant> GetPermissions(string? userid, bool convert = true)
    {
        var query =
            "SELECT DISTINCT grant_name, object_id FROM Grants, permissions, Users WHERE grant_name=permissions.grant_id AND permissions.user_id=Users.user_id ";
        if (convert) query += "AND Users.user_id=sha2(@userid, 256)";
        else query += "AND Users.user_id=@userid";

        var results = Database.Database.ExecuteSelect(
            query,
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object?>
            {
                { "@userid", userid }
            });
        var array = new List<Grant>();
        for (var i = 0; i < results?.Rows.Count; i++)
            array.Add(
                new Grant(
                    results.Rows[i]["grant_name"].ToString() ?? "",
                    int.TryParse(results.Rows[i]["object_id"].ToString(), out var idObject) ? idObject : null
                )
            );
        return array;
    }

    public static string?[] GetAuthorizedAuthors(string? userid)
    {
        var results = Database.Database.ExecuteSelect(
            "SELECT a.* FROM Authors a, permissions p WHERE p.user_id = sha2(@userid, 256) AND a.author_id = p.object_id AND p.grant_id = 'authors'",
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object?>
            {
                { "@userid", userid }
            });
        var array = new string?[results?.Rows.Count ?? 0];
        for (var i = 0; i < results?.Rows.Count; i++) array[i] = results.Rows[i]["name"].ToString();
        return array;
    }

    public static string GetAccountType(JwtSecurityToken jwtSecurityToken)
    {
        var results = Database.Database.ExecuteSelect(
            "SELECT account_type FROM Users WHERE user_id = sha2(@userid, 256)",
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object?>
            {
                { "@userid", jwtSecurityToken.Subject }
            });
        return results?.Rows[0]["account_type"].ToString() ?? "NONE";
    }
}
