using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Middlewares;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace PoliFemoBackend.Source.Utils.Main.WebApplicationUtil;

public static class WebApplicationConfigUtil
{
    internal static void AppConfigPostServerThreads(WebApplication? app)
    {
        if (app == null)
            return;

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

        app.UseResponseCaching();
        
        app.UseAuthorization();

        app.UseUserActivityMiddleware();

        app.MapControllers();
    }

    internal static void AppConfigPreServerThreads(IApplicationBuilder? app)
    {
        if (app == null)
            return;

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
    }
}