#region

using Newtonsoft.Json;
using PoliFemoBackend.Source.Utils;

#endregion

namespace PoliFemoBackend.Source.Test;

public static class Test
{
    public static void TestMain()
    {
        Console.WriteLine("Test");
        DbConfig.InitializeDbConfig();
        var x = Controllers.Articles.ArticleByIdController.SearchArticlesByIdObject(28);
        var json = JsonConvert.SerializeObject(x);
        Console.WriteLine(json);
    }
}