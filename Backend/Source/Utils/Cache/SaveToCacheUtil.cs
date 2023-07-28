using PoliFemoBackend.Source.Data;
using PoliNetwork.Core.Data;
using DB = PoliNetwork.Db.Utils.Database;

namespace PoliFemoBackend.Source.Utils.Cache;

public static class SaveToCacheUtil
{
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
            Variables.DefaultLogger.Error(ex.ToString());
        }
    }
}