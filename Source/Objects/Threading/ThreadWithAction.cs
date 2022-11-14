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
        try
        {
            this._thread.Start();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private void RunThread()
    {
        this._action.Invoke();
    }
}