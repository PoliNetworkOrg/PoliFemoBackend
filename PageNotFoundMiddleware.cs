namespace PoliFemoBackend;

public class PageNotFoundMiddleware
{
    private readonly RequestDelegate _next;

    public PageNotFoundMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        // invoke _next to make the next code run before the response sent
        await _next(httpContext);

        // Make sure the response has not been sent by the controller, 
        // this ensures that the message we are about to send does not 
        // suppress messages from the controller with the same status code
        if (!httpContext.Response.HasStarted)
            switch (httpContext.Response.StatusCode)
            {
                case 404:
                    await httpContext.Response.WriteAsJsonAsync(new
                    {
                        Description =
                            "Welcome to PoliNetwork's API. If you were looking for something in particular it has not been found or has been moved. For a list of available APIs please visit https://api.polinetwork.org/swagger",
                        Status = 404
                    });

                    return;
            }
    }
}