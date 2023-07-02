using PoliFemoBackend.Source.Data;

namespace PoliFemoBackend.Source.Utils.Auth;

public class AccountAuthoursAuthUtil
{
    public static string?[] GetAuthorizedAuthors(string? userid)
    {
        var results = PoliNetwork.Db.Utils.Database.ExecuteSelect(
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
}