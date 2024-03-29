﻿#region

using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Enums;
using PoliFemoBackend.Source.Objects.Permissions;
using DB = PoliNetwork.Db.Utils.Database;

#endregion

namespace PoliFemoBackend.Source.Utils.Auth;

public static class AccountAuthUtil
{
    public static AccountType GetAccountType(string? userid)
    {
        if (userid == null)
            return AccountType.NONE;

        var results = DB.ExecuteSelect(
            "SELECT account_type FROM Users WHERE user_id = sha2(@userid, 256)",
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object?> { { "@userid", userid } }
        );
        return results?.Rows[0]["account_type"].ToString() switch
        {
            "1" => AccountType.POLIMI,
            "2" => AccountType.POLINETWORK,
            "3" => AccountType.PERSONAL,
            _ => AccountType.NONE
        };
    }

    public static List<Grant> GetPermissions(string? userid, bool convert = true)
    {
        var query =
            "SELECT DISTINCT grant_name, object_id FROM Grants, permissions, Users WHERE grant_name=permissions.grant_id AND permissions.user_id=Users.user_id ";
        if (convert)
            query += "AND Users.user_id=sha2(@userid, 256)";
        else
            query += "AND Users.user_id=@userid";

        var results = DB.ExecuteSelect(
            query,
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object?> { { "@userid", userid } }
        );
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
        var results = DB.ExecuteSelect(
            "SELECT grant_id FROM permissions, Grants, Users WHERE Users.user_id=sha2(@userid, 256) AND grant_id=@permission",
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object?> { { "@userid", userid }, { "@permission", permission } }
        );
        return results != null;
    }

    public static bool HasGrantAndObjectPermission(string? userid, string permission, int objectid)
    {
        var results = DB.ExecuteSelect(
            "SELECT grant_id FROM permissions WHERE user_id=sha2(@userid, 256) AND grant_id=@permission AND object_id=@objectid",
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object?>
            {
                { "@userid", userid },
                { "@permission", permission },
                { "@objectid", objectid }
            }
        );
        return results != null;
    }

    public static string?[] GetAuthorizedAuthors(string? userid)
    {
        var results = DB.ExecuteSelect(
            "SELECT a.* FROM Authors a, permissions p WHERE p.user_id = sha2(@userid, 256) AND a.author_id = p.object_id AND p.grant_id = '"
                + Constants.Permissions.ManageArticles
                + "'",
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object?> { { "@userid", userid } }
        );
        var array = new string?[results?.Rows.Count ?? 0];
        for (var i = 0; i < results?.Rows.Count; i++)
            array[i] = results.Rows[i]["name"].ToString();
        return array;
    }
}
