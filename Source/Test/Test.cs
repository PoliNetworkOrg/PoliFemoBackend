#region

using PoliFemoBackend.Source.Data;
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

        FixGlobalDbConfig();

        try
        {
            var r2 = await SingleRoomUtil.GetRoomById(32);
            Console.WriteLine(r2);
            var hourStart = new DateTime(2023, 02, 27, 8, 0, 0);
            var hourStop = new DateTime(2023, 02, 27, 20, 0, 0);
            ;
            var r = await SearchRoomUtil.SearchRooms("MIA", hourStart, hourStop);
            ;
            Console.WriteLine(r);
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