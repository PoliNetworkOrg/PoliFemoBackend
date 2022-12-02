#region

using PoliFemoBackend.Source.Utils.Database;

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