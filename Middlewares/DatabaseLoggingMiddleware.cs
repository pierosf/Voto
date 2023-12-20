using voto.Helpers;
using voto.Repository.Interfaces;

namespace voto.Middlewares;

public class DatabaseLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public DatabaseLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ILogRepository _logRepo)
    {
        await _logRepo.CreateLog(new() {
            Message = $"{context.Request.Path} - {context.Request.Method}",
            Date = DateTime.Now,
            Type = TypeEnum.REQUEST
        });
        await _next(context);
        await _logRepo.CreateLog(new() {
            Message = $"{context.Request.Path} - {context.Request.Method} - {context.Response.StatusCode}",
            Date = DateTime.Now,
            Type = context.Response.StatusCode == 500 ? TypeEnum.ERROR : TypeEnum.RESPONSE
        });
    }
}

public static class DatabaseLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseDatabaseLoggingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<DatabaseLoggingMiddleware>();
    }
}
