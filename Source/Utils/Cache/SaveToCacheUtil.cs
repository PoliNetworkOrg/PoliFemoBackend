using System.Collections;
using PoliFemoBackend.Source.Data;

namespace PoliFemoBackend.Source.Utils.Cache;

public class SaveToCacheUtil
{
    internal static void SaveResultInCache(string urlAddress, bool useCache, string s)
    {
        if (!useCache)
            return;

        try
        {
            var dictionary = new Dictionary<string, object?> { { "@url", urlAddress }, { "@content", s } };
            const string q =
                "INSERT INTO WebCache (url, content, expires_at) VALUES (@url, @content, NOW() + INTERVAL 2 DAY)";
            Database.Database.Execute(q, GlobalVariables.DbConfigVar, dictionary);
        }
        catch
        {
            ;
        }
    }

    internal static void SaveToCache(string polimidailysituation, IEnumerable results)
    {
        try
        {
            const string qi =
                "INSERT INTO WebCache (url, content, expires_at) VALUES (@url, @content, NOW() + INTERVAL 2 DAY)";
            var objects = new Dictionary<string, object?>
            {
                { "@url", polimidailysituation },
                { "@content", results.ToString() }
            };
            Database.Database.Execute(qi, GlobalVariables.DbConfigVar, objects);
        }
        catch (Exception ex)
        {
            Logger.WriteLine(ex);
        }
    }

}