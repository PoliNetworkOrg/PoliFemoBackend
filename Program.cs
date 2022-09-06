#region

using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Test;
using PoliFemoBackend.Source.Utils;

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

            app.UseMiddleware<PageNotFoundMiddleware>();

            app.UseCors(builder =>
            {
                builder
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
        JObject json = new JObject();
        json.Add("error", "Invalid token. Refresh your current access token or request a new authorization code");
        json.Add("reason", context.AuthenticateFailure?.Message);
        context.Response.StatusCode = 401;
        context.Response.ContentType = "application/json";
        

        if (!context.Request.Headers.ContainsKey("Authorization"))
        {
            json["reason"] = "Missing Authorization header";
        } else if (!context.Request.Headers["Authorization"].ToString().StartsWith("Bearer "))
        {
            json["reason"] = "Not a Bearer token";     
        }

        context.HandleResponse();
        await context.Response.WriteAsync(JsonConvert.SerializeObject(json));
    }
}