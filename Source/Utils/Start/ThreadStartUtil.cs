using PoliFemoBackend.Source.Objects.Threading;

namespace PoliFemoBackend.Source.Utils.Start;

public static class ThreadStartUtil
{
    private static ThreadWithAction? _getNewsThreadWithAction;
    
    public static void ThreadStartMethod()
    {
        _getNewsThreadWithAction = new ThreadWithAction();
        _getNewsThreadWithAction.SetAction(() => News.PoliMiNewsUtil.LoopGetNews(_getNewsThreadWithAction));
        _getNewsThreadWithAction.Run();
    }
}