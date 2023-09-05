#region

using System.Net;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Objects.Web;
using DB = PoliNetwork.Db.Utils.Database;

#endregion

namespace PoliFemoBackend.Source.Utils.Cache;

public static class CacheUtil
{
    public static string? GetCache(string urlAddress)
    {
        const string selectFromWebcacheWhereUrlUrl = "SELECT * FROM WebCache WHERE url = @url";
        var dictionary = new Dictionary<string, object?> { { "@url", urlAddress } };
        var q = DB.ExecuteSelect(selectFromWebcacheWhereUrlUrl, GlobalVariables.DbConfigVar, dictionary);
        return q?.Rows.Count > 0 ? q.Rows[0]["content"].ToString() : null;
    }

    internal static void SaveToCache(string url, string content)
    {
        try
        {
            const string qi =
                "INSERT INTO WebCache (url, content, expires_at) VALUES (@url, @content, NOW() + INTERVAL 2 DAY)";
            var objects = new Dictionary<string, object?>
            {
                { "@url", url },
                { "@content", content }
            };
            DB.Execute(qi, GlobalVariables.DbConfigVar, objects);
        }
        catch (Exception ex)
        {
            PoliNetwork.Core.Data.GlobalVariables.DefaultLogger.Error(ex.ToString());
        }
    }

    public static WebReply? CheckIfToUseCache(string urlAddress)
    {
        var sq = CacheUtil.GetCache(urlAddress);
        return sq != null ? new WebReply(sq, HttpStatusCode.OK) : null;
    }
}