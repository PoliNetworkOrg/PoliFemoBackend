#region

using System.Net;
using System.Threading.RateLimiting;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;
using AspNetCore.Proxy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using PoliFemoBackend.Source.Configure;
using PoliFemoBackend.Source.Data;

#endregion

namespace PoliFemoBackend.Source.Utils.Main.WebApplicationUtil;

public static class CreateApplicationUtil
{
    internal static WebApplication? CreateWebApplication(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder
            .Services
            .Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
        builder
            .Services
            .Configure<MvcOptions>(options =>
            {
                options.EnableEndpointRouting = false;
            });

        builder
            .Services
            .AddMvcCore(
                opts => opts.Filters.Add(new MetricsResourceFilter(new MvcRouteTemplateResolver()))
            );
        builder.Services.AddLogging();
        builder.Services.AddProxies();

        var metrics = AppMetrics.CreateDefaultBuilder().Build();

        builder.Services.AddMetrics(metrics);

        builder
            .Services
            .AddCors(options =>
            {
                options.AddPolicy(
                    "policy",
                    policy =>
                    {
                        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                    }
                );
            });

        builder
            .Services
            .AddRateLimiter(limiterOptions =>
            {
                limiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                limiterOptions.GlobalLimiter = PartitionedRateLimiter.Create<
                    HttpContext,
                    IPAddress
                >(context =>
                {
                    IPAddress? remoteIpAddress = context.Connection.RemoteIpAddress;
                    if (!IPAddress.IsLoopback(remoteIpAddress!))
                    {
                        return RateLimitPartition.GetTokenBucketLimiter(
                            remoteIpAddress!,
                            _ =>
                                new TokenBucketRateLimiterOptions
                                {
                                    TokenLimit = 50,
                                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                                    QueueLimit = 100,
                                    ReplenishmentPeriod = TimeSpan.FromMinutes(1),
                                    TokensPerPeriod = 50,
                                    AutoReplenishment = true
                                }
                        );
                    }
                    return RateLimitPartition.GetNoLimiter(IPAddress.Loopback);
                });
            });
        builder
            .Host
            .ConfigureMetrics(metricsBuilder =>
            {
                metricsBuilder
                    .Configuration
                    .Configure(options =>
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
                    endpointsOptions.MetricsTextEndpointOutputFormatter =
                        new MetricsPrometheusTextOutputFormatter();
                    endpointsOptions.MetricsEndpointEnabled = false;
                };
            });

        builder.Services.AddControllers().AddNewtonsoftJson();

        //https://learn.microsoft.com/en-us/aspnet/core/performance/caching/response?view=aspnetcore-7.0
        builder.Services.AddResponseCaching();

        builder
            .Services
            .AddApiVersioning(setup =>
            {
                setup.ApiVersionReader = new UrlSegmentApiVersionReader();
                setup.DefaultApiVersion = new ApiVersion(1, 0);
                setup.AssumeDefaultVersionWhenUnspecified = true;
                setup.ReportApiVersions = true;
            });
        builder
            .Services
            .AddVersionedApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });

        builder.Services.AddSwaggerGen();

        builder
            .Services
            .AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = Constants.AzureAuthority;
                options.TokenValidationParameters.ValidAudience = Constants.AzureClientId;
                options.TokenValidationParameters.ValidIssuers = new[]
                {
                    Constants.AzureCommonIssuer,
                    Constants.AzurePolimiIssuer,
                    Constants.AzurePoliNetworkIssuer
                };
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        await ProgramUtil.OnChallengeMethod(context);
                    }
                };
            });

        builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

        var app = builder.Build();
        return app;
    }
}
