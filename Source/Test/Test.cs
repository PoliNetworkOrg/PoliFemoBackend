﻿#region

using Newtonsoft.Json;
using PoliFemoBackend.Source.Utils.Database;

#endregion

namespace PoliFemoBackend.Source.Test;

public static class Test
{
    public static void TestMain()
    {
        Console.WriteLine("Test");

        DbConfig.InitializeDbConfig();
        var x = Utils.News.PoliMi.DownloadNewsUtil.DownloadCurrentNews().ToList();
        var json = JsonConvert.SerializeObject(x);
        Console.WriteLine(json);
    }
}