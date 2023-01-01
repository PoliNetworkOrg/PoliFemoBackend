using Microsoft.AspNetCore.Authorization;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils.Database;

namespace PoliFemoBackend.UserActivityMiddleware;

public class UserActivityMiddleware {
    private readonly RequestDelegate _next;

    public UserActivityMiddleware(RequestDelegate next) {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext) {
        if (httpContext.GetEndpoint()?.Metadata?.GetMetadata<AuthorizeAttribute>() is object) {
            var token = httpContext.Request.Headers["Authorization"].ToString().Split(" ")[1];
            var handler = GlobalVariables.TokenHandler?.ReadJwtToken(token);

            var query = "UPDATE Users SET last_activity = NOW() WHERE id_user = SHA2(@subject, 256)";
            var parameters = new Dictionary<string, object?>
            {
                {"@subject", handler?.Subject},
            };
            var results = Database.Execute(query, GlobalVariables.DbConfigVar, parameters);
            if (results == 0) {
                //send an error message
                httpContext.Response.StatusCode = 500;
                await httpContext.Response.WriteAsJsonAsync(new
                {
                    error =
                        "An error occurred while updating the user info. Please try again later.",
                });
            }
            else
                await _next(httpContext);

        }
        else
            await _next(httpContext);
    }
}

public static class UserActivityMiddlewareExtensions {
    public static IApplicationBuilder UseUserActivityMiddleware(this IApplicationBuilder builder) {
        return builder.UseMiddleware<UserActivityMiddleware>();
    }
}