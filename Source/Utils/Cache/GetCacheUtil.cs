using System.Net;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Objects.Web;

namespace PoliFemoBackend.Source.Utils.Cache;

public static class GetCacheUtil
{
    public static string? GetCache(string urlAddress)
    {
        const string selectFromWebcacheWhereUrlUrl = "SELECT * FROM WebCache WHERE url = @url";
        var dictionary = new Dictionary<string, object?> { { "@url", urlAddress } };
        var q = Database.Database.ExecuteSelect(selectFromWebcacheWhereUrlUrl, GlobalVariables.DbConfigVar, dictionary);
        return q?.Rows.Count > 0 ? q.Rows[0]["content"].ToString() : null;
    }
}