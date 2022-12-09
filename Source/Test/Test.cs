#region

using PoliFemoBackend.Source.Utils.Database;
using PoliFemoBackend.Source.Utils.Temp.Migrate;

#endregion

namespace PoliFemoBackend.Source.Test;

public static class Test
{
    public static void TestMain()
    {
        Console.WriteLine("Test");

        DbConfig.InitializeDbConfig();
        ArticleContentUpgrade.ArticleContentUpgradeMethod();
    }
}