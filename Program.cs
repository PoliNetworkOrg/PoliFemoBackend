#region

using System.IdentityModel.Tokens.Jwt;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Configure;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Middlewares;
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

        GlobalVariables.BasePath = args.FirstOrDefault(arg => arg.StartsWith("--base-path="))?.Split('=')[1] ?? "/";
        var useNews = !args.Any(arg => arg == "--no-news");

        try
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.Configure<KestrelServerOptions>(options => { options.AllowSynchronousIO = true; });
            builder.Services.Configure<MvcOptions>(options => { options.EnableEndpointRouting = false; });

            builder.Services.AddMvcCore(opts =>
                opts.Filters.Add(new MetricsResourceFilter(new MvcRouteTemplateResolver())));
            builder.Services.AddLogging();

            var metrics = AppMetrics.CreateDefaultBuilder().Build();

            builder.Services.AddMetrics(metrics);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("policy",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            builder.Host
                .ConfigureMetrics(metricsBuilder =>
                {
                    metricsBuilder.Configuration.Configure(options => { options.DefaultContextLabel = "default"; });
                })
                .UseMetricsWebTracking()
                .UseMetricsEndpoints()
                .UseMetrics(options =>
                {
                    options.EndpointOptions = endpointsOptions =>
                    {
                        endpointsOptions.MetricsTextEndpointOutputFormatter =
                            new MetricsPrometheusTextOutputFormatter();
                        endpointsOptions.MetricsEndpointEnabled = false;
                    };
                });

            builder.Services.AddControllers().AddNewtonsoftJson();

            builder.Services.AddApiVersioning(setup =>
            {
                setup.DefaultApiVersion = new ApiVersion(1, 0);
                setup.AssumeDefaultVersionWhenUnspecified = true;
                setup.ReportApiVersions = true;
            });
            builder.Services.AddVersionedApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });

            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = Constants.AzureAuthority;
                options.TokenValidationParameters.ValidAudience = Constants.AzureClientId;
                options.TokenValidationParameters.ValidIssuers =
                    new[] { Constants.AzureCommonIssuer, Constants.AzureOrgIssuer };
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context => { await OnChallengeMethod(context); }
                };
            });

            builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

            var app = builder.Build();

            GlobalVariables.TokenHandler = new JwtSecurityTokenHandler();

            if (GlobalVariables.BasePath != "/")
            {
                app.UsePathBase(GlobalVariables.BasePath);
                app.UseRouting();
            }

            app.UseSwagger();
            app.UseStaticFiles();
            app.UseSwaggerUI(options =>
            {
                options.DocExpansion(DocExpansion.None);
                options.SwaggerEndpoint(GlobalVariables.BasePath + "swagger/definitions/swagger.json", "PoliFemo API");
                options.InjectStylesheet(GlobalVariables.BasePath + "swagger-ui/SwaggerDark.css");
            });

            try
            {
                Start.StartThings(useNews);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            app.UseMetricsTextEndpoint();
            app.UseMetricsAllMiddleware();

            app.UseMiddleware<PageNotFoundMiddleware>();

            app.UseCors(policyBuilder =>
            {
                policyBuilder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseUserActivityMiddleware();

            app.MapControllers();

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