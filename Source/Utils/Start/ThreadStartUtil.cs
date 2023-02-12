﻿using PoliFemoBackend.Source.Objects.Threading;
using PoliFemoBackend.Source.Utils.News.PoliMi;

namespace PoliFemoBackend.Source.Utils.Start;

public static class ThreadStartUtil
{
    private static ThreadWithAction? _getNewsThreadWithAction;
    private static ThreadWithAction? _checkInactivityThreadWithAction;

    public static void ThreadStartMethod(bool useNews)
    {
        if (useNews)
        {
            _getNewsThreadWithAction = new ThreadWithAction();
            _getNewsThreadWithAction.SetAction(() => PoliMiNewsUtil.LoopGetNews(_getNewsThreadWithAction));
            _getNewsThreadWithAction.Run();
        }
        else
        {
            Logger.WriteLine("--no-news flag found. We will not search for news.", LogSeverityLevel.Info);
        }
        
        _checkInactivityThreadWithAction = new ThreadWithAction();
        _checkInactivityThreadWithAction.SetAction(() => Controllers.Accounts.AccountAutoExpireAfterInactivity.LoopCheckInactivity(_checkInactivityThreadWithAction));
        _checkInactivityThreadWithAction.Run();
    }
}