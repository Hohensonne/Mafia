using System;

namespace Mafia.API;

public class ExeptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExeptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await context.Response.WriteAsync(ex.Message);
        }
    }
}


public static class ExeptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExeptionHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExeptionHandlingMiddleware>();
    }
}
