#region

using System.IdentityModel.Tokens.Jwt;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Configure;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Middleware;
using PoliFemoBackend.Source.Test;
using PoliFemoBackend.Source.Utils.Start;
using Swashbuckle.AspNetCore.SwaggerUI;

#endregion

namespace PoliFemoBackend;

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
            Test.TestMain();
            return;
        }

        try
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private static async Task OnChallengeMethod(JwtBearerChallengeContext context)
    {
        var json = new JObject
        {
            { "error", "Invalid token. Refresh your current access token or request a new authorization code" },
            { "reason", context.AuthenticateFailure?.Message }
        };
        context.Response.StatusCode = 401;
        context.Response.ContentType = "application/json";


        if (!context.Request.Headers.ContainsKey(Constants.Authorization))
            json["reason"] = "Missing Authorization header";
        else if (!context.Request.Headers[Constants.Authorization].ToString().StartsWith("Bearer "))
            json["reason"] = "Not a Bearer token";

        context.HandleResponse();
        await context.Response.WriteAsync(JsonConvert.SerializeObject(json));
    }
}