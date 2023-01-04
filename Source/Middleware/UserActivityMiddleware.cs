using Microsoft.AspNetCore.Authorization;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils.Database;

namespace PoliFemoBackend.Source.Middleware;

public class UserActivityMiddleware
{
    private readonly RequestDelegate _next;

    public UserActivityMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        var done = TryUpdateLastActivity(httpContext);

        if (!done)
        {
            //send an error message
            httpContext.Response.StatusCode = 500;
            await httpContext.Response.WriteAsJsonAsync(new
            {
                error =
                    "An error occurred while updating the user info. Please try again later."
            });
        }
        else
        {
            await _next(httpContext);
        }
    }

    private static bool TryUpdateLastActivity(HttpContext httpContext)
    {
        var endpointMetadataCollection = httpContext.GetEndpoint()?.Metadata;
        var authorizeAttribute = endpointMetadataCollection?.GetMetadata<AuthorizeAttribute>();

        if (authorizeAttribute == null)
            return true;

        var token = httpContext.Request.Headers["Authorization"].ToString().Split(" ")[1];
        var handler = GlobalVariables.TokenHandler?.ReadJwtToken(token);
        var handlerSubject = handler?.Subject;

        if (string.IsNullOrEmpty(handlerSubject))
            return false;

        const string query = "UPDATE Users SET last_activity = NOW() WHERE id_user = SHA2(@subject, 256)";
        var parameters = new Dictionary<string, object?>
        {
            { "@subject", handlerSubject }
        };
        
        var results = Database.Execute(query, GlobalVariables.DbConfigVar, parameters);
        return results > 0;
    }
}

public static class UserActivityMiddlewareExtensions
{
    public static IApplicationBuilder UseUserActivityMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<UserActivityMiddleware>();
    }
}