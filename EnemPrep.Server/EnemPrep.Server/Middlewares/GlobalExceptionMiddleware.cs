namespace EnemPrep.Server.Middlewares;

public class GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger, IWebHostEnvironment env) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception has occurred.");

            if (context.Response.HasStarted)
            {
                throw;
            }
            
            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new
            {
                error = "Internal Server Error",
                traceId = context.TraceIdentifier,
                details = env.IsDevelopment() ? ex.Message : null
            };
            
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}