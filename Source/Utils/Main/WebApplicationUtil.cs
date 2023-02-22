﻿using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using PoliFemoBackend.Source.Configure;
using PoliFemoBackend.Source.Data;

namespace PoliFemoBackend.Source.Utils.Main;

public static class WebApplicationUtil
{
    internal static WebApplication CreateWebApplication(string[] args)
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
            setup.ApiVersionReader = new UrlSegmentApiVersionReader();
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
                new[]
                {
                    Constants.AzureCommonIssuer,
                    Constants.AzurePolimiIssuer,
                    Constants.AzurePoliNetworkIssuer
                };
            options.Events = new JwtBearerEvents
            {
                OnChallenge = async context => { await Utils.Main.ProgramUtil.OnChallengeMethod(context); }
            };
        });

        builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

        var app = builder.Build();
        return app;
    }
}