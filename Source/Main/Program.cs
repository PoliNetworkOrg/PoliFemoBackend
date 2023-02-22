#region

using System.IdentityModel.Tokens.Jwt;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Configure;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Middlewares;
using PoliFemoBackend.Source.Utils;
using PoliFemoBackend.Source.Utils.Start;
using Swashbuckle.AspNetCore.SwaggerUI;

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
        var app = Utils.Main.WebApplicationUtil.CreateWebApplication(args);

        GlobalVariables.TokenHandler = new JwtSecurityTokenHandler();

        Utils.Main.WebApplicationUtil.AppConfigPreServerThreads(app);

        try
        {
            Start.StartThings(au.UseNews);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        Utils.Main.WebApplicationUtil.AppConfigPostServerThreads(app);

        app.Run();
    }
}