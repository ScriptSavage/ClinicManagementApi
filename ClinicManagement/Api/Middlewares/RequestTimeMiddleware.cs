using System.Diagnostics;

namespace Api.Middlewares;

public class RequestTimeMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await next(context);
        }
        finally
        {
            stopwatch.Stop();
        }

        if (stopwatch.Elapsed > TimeSpan.FromSeconds(10))
        {
            throw new TimeoutException("Request took too long to complete");
        }
    }
}
