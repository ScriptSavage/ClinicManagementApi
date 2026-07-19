using ApplicationCore.Exceptions;
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
        catch (AlreadyExistsException e)
        {
            context.Response.StatusCode = StatusCodes.Status409Conflict;
            await context.Response.WriteAsync(e.Message);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            Console.WriteLine(ex.Message);
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 500;
            Console.WriteLine(e.Message);
        }
    }
}