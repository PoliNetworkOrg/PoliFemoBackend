using System.IdentityModel.Tokens.Jwt;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Objects.Permissions;

namespace PoliFemoBackend.Source.Utils.Auth;

public static class AccountAuthUtil
{
    public static string GetAccountType(JwtSecurityToken jwtSecurityToken)
    {
        var results = PoliNetwork.Db.Utils.Database.ExecuteSelect(
            "SELECT account_type FROM Users WHERE user_id = sha2(@userid, 256)",
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object?>
            {
                { "@userid", jwtSecurityToken.Subject }
            });
        return results?.Rows[0]["account_type"].ToString() ?? "NONE";
    }

    public static List<Grant> GetPermissions(string? userid, bool convert = true)
    {
        var query =
            "SELECT DISTINCT grant_name, object_id FROM Grants, permissions, Users WHERE grant_name=permissions.grant_id AND permissions.user_id=Users.user_id ";
        if (convert) query += "AND Users.user_id=sha2(@userid, 256)";
        else query += "AND Users.user_id=@userid";

        var results = PoliNetwork.Db.Utils.Database.ExecuteSelect(
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
                    int.TryParse(results.Rows[i]["object_id"].ToString(), out var id) ? id : -1
                )
            );
        return array;
    }

    public static bool HasPermission(string? userid, string permission)
    {
        var results = PoliNetwork.Db.Utils.Database.ExecuteSelect(
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
        var results = PoliNetwork.Db.Utils.Database.ExecuteSelect(
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
}