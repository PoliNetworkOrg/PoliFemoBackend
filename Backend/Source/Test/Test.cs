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
            Task task = TestMain();
            task.Wait();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private static Task TestMain()
    {
        Console.WriteLine("Test");

        //FixGlobalDbConfig();

        try
        {
            var r2 = DownloadNewsUtil.DownloadCurrentNews();
            var newsPolimis = r2.ToList();
            Console.WriteLine(newsPolimis.Count);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }


        //DbConfig.InitializeDbConfig();
        //ArticleContentUpgrade.ArticleContentUpgradeMethod();
        return Task.CompletedTask;
    }
}