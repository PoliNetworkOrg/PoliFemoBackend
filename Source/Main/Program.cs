#region

using System.IdentityModel.Tokens.Jwt;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils;
using PoliFemoBackend.Source.Utils.Main;
using PoliFemoBackend.Source.Utils.Start;

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
        if (args.Length > 0 && args[0] == "test")
        {
            Test.Test.RunTest();
            return;
        }

        RunServer(args);
    }

    private static void RunServer(string[] args)
    {
        var au = new ArgumentsUtil(args);

        try
        {
            StartServer(args, au);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }


    private static void StartServer(string[] args, ArgumentsUtil au)
    {
        var app = WebApplicationUtil.CreateWebApplication(args);

        GlobalVariables.TokenHandler = new JwtSecurityTokenHandler();

        WebApplicationUtil.AppConfigPreServerThreads(app);

        try
        {
            Start.StartThings(au.UseNews);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        WebApplicationUtil.AppConfigPostServerThreads(app);

        app.Run();
    }
}