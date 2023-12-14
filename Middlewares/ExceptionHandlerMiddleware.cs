using voto.Entities;
using voto.Exceptions;
using voto.Helpers;
using voto.Repository.Interfaces;

namespace voto.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogRepository _logRepo;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogRepository logRepo)
    {
        _next = next;
        _logRepo = logRepo;
    }

    public async Task Invoke(HttpContext context)
    {
        try 
        {
            await _next(context);
        }
        catch(CustomException ex)
        {
            context.Response.StatusCode = ex.StatusCode;
            await context.Response.WriteAsJsonAsync(new ErrorEntity() {Message = ex.Message, StatusCode = ex.StatusCode});
        }
    }
}

public static class ExceptionHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}