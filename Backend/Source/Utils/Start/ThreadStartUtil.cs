#region

using PoliFemoBackend.Source.Utils.News.PoliMi;
using PoliFemoBackend.Source.Utils.Rooms;
using PoliNetwork.Core.Data;
using PoliNetwork.Core.Objects.Threading;

#endregion

namespace PoliFemoBackend.Source.Utils.Start;

public static class ThreadStartUtil
{
    private static ThreadWithAction? _getNewsThreadWithAction;
    private static ThreadWithAction? _roomsSearchThreadWithAction;

    public static void ThreadStartMethod(bool useNews, bool searchRooms)
    {
        if (useNews)
        {
            _getNewsThreadWithAction = new ThreadWithAction();
            _getNewsThreadWithAction.SetAction(
                () => PoliMiNewsUtil.LoopGetNews(_getNewsThreadWithAction)
            );
            _getNewsThreadWithAction.Run();
        }
        else
        {
            GlobalVariables.DefaultLogger.Info(
                "--no-news flag found. We will not search for news."
            );
        }

        if (searchRooms)
        {
            _roomsSearchThreadWithAction = new ThreadWithAction();
            _roomsSearchThreadWithAction.SetAction(
                () => SearchRoomUtil.LoopSearchRooms(_roomsSearchThreadWithAction)
            );
            _roomsSearchThreadWithAction.Run();
        }
        else
        {
            GlobalVariables.DefaultLogger.Info(
                "--no-rooms flag found. We will not search for free rooms."
            );
        }
    }
}
