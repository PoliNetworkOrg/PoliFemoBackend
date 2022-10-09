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
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Test;
using PoliFemoBackend.Source.Utils;
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
            builder.Services.Configure<KestrelServerOptions>(options => { options.AllowSynchronousIO = true; });
            builder.Services.Configure<MvcOptions>(options => { options.EnableEndpointRouting = false; });

            builder.Services.AddMvcCore(opts => opts.Filters.Add(new MetricsResourceFilter(new MvcRouteTemplateResolver())));
            builder.Services.AddLogging();

            var metrics = AppMetrics.CreateDefaultBuilder().Build();

            builder.Services.AddMetrics(metrics);

            builder.Host
            .ConfigureMetrics(builder =>
                {
                    builder.Configuration.Configure(options =>
                        {
                            options.DefaultContextLabel = "default";
                        });
                })
            .UseMetricsWebTracking()
            .UseMetricsEndpoints()
            .UseMetrics(options =>
                {
                    options.EndpointOptions = endpointsOptions =>
                        {
                            endpointsOptions.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
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
                options.TokenValidationParameters.ValidAudience = Constants.AzureAudience;
                options.TokenValidationParameters.ValidIssuer = Constants.AzureIssuer;
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context => { await OnChallengeMethod(context); }
                };
            });

            builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

            var app = builder.Build();

            GlobalVariables.TokenHandler = new JwtSecurityTokenHandler();

            app.UseSwagger();
            app.UseStaticFiles();
            app.UseSwaggerUI(options =>
            {
                options.DocExpansion(DocExpansion.None);
                if (app.Services.GetService(typeof(IApiVersionDescriptionProvider)) is IApiVersionDescriptionProvider
                    provider)
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint("/swagger/" + description.GroupName + "/swagger.json",
                            "PoliFemoBackend API " + description.GroupName.ToUpperInvariant());
                        options.InjectStylesheet("/swagger-ui/SwaggerDark.css");
                        options.RoutePrefix = "swagger";
                    }
                }
                else
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "PoliFemoBackend API V1");
                    options.RoutePrefix = "swagger";
                }
            });

            try
            {
                Start.StartThings();
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


        if (!context.Request.Headers.ContainsKey("Authorization"))
            json["reason"] = "Missing Authorization header";
        else if (!context.Request.Headers["Authorization"].ToString().StartsWith("Bearer "))
            json["reason"] = "Not a Bearer token";

        context.HandleResponse();
        await context.Response.WriteAsync(JsonConvert.SerializeObject(json));
    }
}