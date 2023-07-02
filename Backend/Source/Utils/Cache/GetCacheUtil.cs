using PoliFemoBackend.Source.Data;

namespace PoliFemoBackend.Source.Utils.Cache;

public static class GetCacheUtil
{
    public static string? GetCache(string urlAddress)
    {
        const string selectFromWebcacheWhereUrlUrl = "SELECT * FROM WebCache WHERE url = @url";
        var dictionary = new Dictionary<string, object?> { { "@url", urlAddress } };
        var q = PoliNetwork.Db.Utils.Database.ExecuteSelect(selectFromWebcacheWhereUrlUrl, GlobalVariables.DbConfigVar, dictionary);
        return q?.Rows.Count > 0 ? q.Rows[0]["content"].ToString() : null;
    }
}