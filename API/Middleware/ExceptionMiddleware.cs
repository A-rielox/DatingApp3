using API.Errors;
using System.Net;
using System.Text.Json;

namespace API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    // los middlewares necesitan el delegate para pasar al next
    // IHostEnvironment env --> p' ver en q ambiente estoy
    public ExceptionMiddleware(
                                RequestDelegate next,
                                ILogger<ExceptionMiddleware> logger,
                                IHostEnvironment env
                                )
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    // con "HttpContext context" es q tengo acceso al req
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message); // p' ver el error en la terminal
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // ApiException es la clase q yo cree
            var response = _env.IsDevelopment()
                ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                : new ApiException(context.Response.StatusCode, "Internal Server Error");

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
    }
}
