using FluentValidation;

namespace Api.Middlewares;

public class GlobalErrorHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch(ValidationException ex)
        {
           context.Response.StatusCode =  StatusCodes.Status400BadRequest;
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 500;
        }
    }
}