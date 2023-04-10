#region

using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils.News.PoliMi;

#endregion

namespace PoliFemoBackend.Source.Test;

public static class Test
{
    internal static void RunTest()
    {
        try
        {
            var task = TestMain();
            task.Wait();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private static async Task TestMain()
    {
        Console.WriteLine("Test");

        //FixGlobalDbConfig();

        try
        {
            var r2 = DownloadNewsUtil.DownloadCurrentNews();
            var newsPolimis = r2.ToList();
            Console.WriteLine(newsPolimis.Count);
            var enumerable = newsPolimis.Select(variable => variable.GetContentAsTextJson());
            var objects = enumerable.Select(r3 => r3?.ToString());
            foreach (var r4 in objects)
            {
                Console.WriteLine(r4);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }


        //DbConfig.InitializeDbConfig();
        //ArticleContentUpgrade.ArticleContentUpgradeMethod();
    }

    private static void FixGlobalDbConfig()
    {
        GlobalVariables.DbConfigVar ??= GlobalVariables.GetDbConfig();
    }
}