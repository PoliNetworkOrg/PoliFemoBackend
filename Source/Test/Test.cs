#region

using Newtonsoft.Json;
using PoliFemoBackend.Source.Objects.Article.News;
using PoliFemoBackend.Source.Utils.Database;
using PoliFemoBackend.Source.Utils.News.PoliMi;

#endregion

namespace PoliFemoBackend.Source.Test;

public static class Test
{
    public static void TestMain()
    {
        Console.WriteLine("Test");

        DbConfig.InitializeDbConfig();
        Utils.Temp.Migrate.ArticleContentUpgrade.ArticleContentUpgradeMethod();
        
    }
}