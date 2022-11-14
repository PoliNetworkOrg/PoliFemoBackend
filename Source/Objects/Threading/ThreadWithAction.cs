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
            _thread?.Start();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private void RunThread()
    {
        _action?.Invoke();
    }

    public void SetAction(Action action)
    {
        _action = action;
        _thread = new Thread(RunThread);
    }
}