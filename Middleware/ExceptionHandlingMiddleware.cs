using EcommerceMVC.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace EcommerceMVC.Middleware;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            logger.LogWarning(ex, "Validation error");
            context.Response.Redirect($"/Home/ErrorMessage?message={Uri.EscapeDataString(ex.Message)}");
        }
        catch (NotFoundException ex)
        {
            logger.LogWarning(ex, "Resource not found");
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            context.Response.Redirect($"/Home/ErrorMessage?message={Uri.EscapeDataString(ex.Message)}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");
            context.Response.Redirect("/Home/Error");
        }
    }
}
