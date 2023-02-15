﻿#region

using PoliFemoBackend.Source.Controllers.Rooms;
using PoliFemoBackend.Source.Utils;
using PoliFemoBackend.Source.Utils.Database;
using PoliFemoBackend.Source.Utils.Rooms;
using PoliFemoBackend.Source.Utils.Temp.Migrate;

#endregion

namespace PoliFemoBackend.Source.Test;

public static class Test
{
    public static async Task TestMain()
    {
        Console.WriteLine("Test");
        
        try
        {
            var hourStart = new DateTime(2023,02,15, 14,30,0);
            var hourStop = new DateTime(2023, 02, 15, 18, 0, 0);
            var r = await SearchRoomUtil.SearchRooms("MIB", hourStart, hourStop);
            Console.WriteLine(r);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }



        ;
        
        //DbConfig.InitializeDbConfig();
        //ArticleContentUpgrade.ArticleContentUpgradeMethod();
    }
}