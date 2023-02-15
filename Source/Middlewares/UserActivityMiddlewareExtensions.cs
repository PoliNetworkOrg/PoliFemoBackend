namespace PoliFemoBackend.Source.Middlewares;

public static class UserActivityMiddlewareExtensions
{
    public static IApplicationBuilder UseUserActivityMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<UserActivityMiddleware>();
    }
}