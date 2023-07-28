#region

using PoliFemoBackend.Source.Utils;
using PoliFemoBackend.Source.Utils.Main;

#endregion

namespace PoliFemoBackend.Source.Main;

internal static class Program
{
    /// <summary>
    ///     The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main(string[] args)
    {
        //FirstThingsToDo();

        if (args.Length > 0 && args[0] == "test")
        {
            Test.Test.RunTest();
            return;
        }

        RunServer(args);
    }

    //private static void FirstThingsToDo()
    //{
    //    PoliNetwork.Db.Utils.LoggerDb.Logger = Logger.LogQuery;
    //}

    private static void RunServer(string[] args)
    {
        var au = new ArgumentsUtil(args);

        try
        {
            StartServerUtil.StartServer(args, au);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}