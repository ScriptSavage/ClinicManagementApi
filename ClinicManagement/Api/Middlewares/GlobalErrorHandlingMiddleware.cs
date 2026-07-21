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
        }
        catch (ValidationException ex)
        {
            await context.Response.WriteAsync(ex.Message);
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 500;
        }
    }
}