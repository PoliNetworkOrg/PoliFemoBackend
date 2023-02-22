using PoliFemoBackend.Source.Data;

namespace PoliFemoBackend.Source.Utils.Danger;

public static class GracefullyShutdown
{
    public static void Shutdown()
    {
        //todo: add code to do a safe shutdown (like save things)
        GlobalVariables.App?.StopAsync().Wait();
        Environment.Exit(0);
    }
}