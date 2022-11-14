namespace PoliFemoBackend.Source.Objects.Threading;

public class ThreadWithAction
{
    private readonly Thread _thread;
    private readonly Action _action;

    public ThreadWithAction(Action action)
    {
        this._action = action;
        this._thread = new Thread(RunThread);
    }

    public void Run()
    {
        this._thread.Start();
    }

    private void RunThread()
    {
        this._action.Invoke();
    }
}