using PoliFemoBackend.Source.Objects.Threading;
<<<<<<< HEAD
=======
using PoliFemoBackend.Source.Utils.News;
>>>>>>> dev2

namespace PoliFemoBackend.Source.Utils.Start;

public static class ThreadStartUtil
{
    private static ThreadWithAction? _getNewsThreadWithAction;
<<<<<<< HEAD
    
    public static void ThreadStartMethod()
    {
        _getNewsThreadWithAction = new ThreadWithAction();
        _getNewsThreadWithAction.SetAction(() => News.PoliMiNewsUtil.LoopGetNews(_getNewsThreadWithAction));
=======

    public static void ThreadStartMethod()
    {
        _getNewsThreadWithAction = new ThreadWithAction();
        _getNewsThreadWithAction.SetAction(() => PoliMiNewsUtil.LoopGetNews(_getNewsThreadWithAction));
>>>>>>> dev2
        _getNewsThreadWithAction.Run();
    }
}