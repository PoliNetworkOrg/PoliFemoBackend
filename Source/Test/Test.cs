﻿#region

using PoliFemoBackend.Source.Utils.Rooms;

#endregion

namespace PoliFemoBackend.Source.Test;

public static class Test
{
    internal static void RunTest()
    {
        var task = TestMain();
        task.Wait();
    }

    private static Task TestMain()
    {
        Console.WriteLine("Test");

        try
        {
            var hourStart = new DateTime(2023, 02, 15, 14, 30, 0);
            var hourStop = new DateTime(2023, 02, 15, 18, 0, 0);
            var r = SearchRoomUtil.SearchRooms("MIB", hourStart, hourStop);
            Console.WriteLine(r);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return Task.CompletedTask;


        //DbConfig.InitializeDbConfig();
        //ArticleContentUpgrade.ArticleContentUpgradeMethod();
    }
}