using PoliFemoBackend.Source.Objects.Threading;
using PoliFemoBackend.Source.Utils.News;

namespace PoliFemoBackend.Source.Utils.Start;

public static class ThreadStartUtil
{
    private static ThreadWithAction? _getNewsThreadWithAction;


    public static void ThreadStartMethod()
    {
        _getNewsThreadWithAction = new ThreadWithAction();
        _getNewsThreadWithAction.SetAction(() => PoliMiNewsUtil.LoopGetNews(_getNewsThreadWithAction));
        _getNewsThreadWithAction.Run();
    }
}