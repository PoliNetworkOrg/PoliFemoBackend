#region

using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Objects.Articles.News;
using PoliFemoBackend.Source.Utils.Rooms;
using PoliFemoBackend.Source.Utils.Rooms.Search;

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
            var r2 =  Utils.News.PoliMi.DownloadNewsUtil.DownloadCurrentNews();
            foreach (var VARIABLE in r2)
            {
                Console.WriteLine(VARIABLE.GetContentAsTextJson());
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