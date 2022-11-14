namespace PoliFemoBackend.Source.Objects.Threading;

public class ThreadWithAction
{
    private Thread? _thread;
    private Action? _action;
    public int Failed = 0;


    public void Run()
    {
        try
        {
            this._thread?.Start();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private void RunThread()
    {
        this._action?.Invoke();
    }

    public void SetAction(Action action)
    {
        this._action = action;
        this._thread = new Thread(RunThread);
    }
}