using PoliFemoBackend.Source.Data;

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
            Database.Database.Execute(qi, GlobalVariables.DbConfigVar, objects);
        }
        catch (Exception ex)
        {
            Logger.WriteLine(ex);
        }
    }
}