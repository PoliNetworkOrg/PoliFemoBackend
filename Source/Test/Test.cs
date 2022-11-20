#region

using Newtonsoft.Json;
using PoliFemoBackend.Source.Controllers.Articles;
using PoliFemoBackend.Source.Utils.Database;

#endregion

namespace PoliFemoBackend.Source.Test;

public static class Test
{
    public static void TestMain()
    {
        Console.WriteLine("Test");

        DbConfig.InitializeDbConfig();
        var x = ArticlesByDateTimeRange.SearchArticlesByDateRangeAsJobject("2022-11-10", null);
        var json = JsonConvert.SerializeObject(x);
        Console.WriteLine(json);
    }
}