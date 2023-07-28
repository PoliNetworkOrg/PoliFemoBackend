#region

using PoliFemoBackend.Source.Data;

#endregion

namespace PoliFemoBackend.Source.Utils.Danger;

public static class GracefullyShutdown
{
    public static void ShutdownGracefully()
    {
        //todo: save things

        if (ServerStop())
            Environment.Exit(0);
    }

    private static bool ServerStop()
    {
        try
        {
            GlobalVariables.App?.StopAsync().Wait();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        return false;
    }
}