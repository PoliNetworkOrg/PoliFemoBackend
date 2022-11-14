using PoliFemoBackend.Source.Objects.Threading;

namespace PoliFemoBackend.Source.Utils.Start;

public static class ThreadStartUtil
{
    private static ThreadWithAction? _getNewsThreadWithAction;
    
    public static void ThreadStartMethod()
    {
        _getNewsThreadWithAction = new ThreadWithAction(Utils.News.PoliMiNewsUtil.LoopGetNews);
        _getNewsThreadWithAction.Run();
    }
}