using Microsoft.AspNetCore.Diagnostics;

namespace Performance.API.Exceptions
{
    public class GlobalExceptionHandler (ILogger<GlobalExceptionHandler> logger)
        : IExceptionHandler
    {

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(
                exception,                          
                "Unhandled exception. TraceId: {TraceId} | Timestamp: {Timestamp} | Method: {Method} | Path: {Path}",
                httpContext.TraceIdentifier,
                DateTime.UtcNow.ToLocalTime(),
                httpContext.Request.Method,
                httpContext.Request.Path);

            var problem = new AppProblemDetails()
            {
                Detail = exception.Message,
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                TraceId = httpContext.TraceIdentifier,
                Errors = []
            };

            switch (exception)
            {
                case ArgumentException:
                    problem.Status = StatusCodes.Status400BadRequest;
                    problem.Title = "Invalid request";
                    break;

                case InvalidOperationException:
                    problem.Status = StatusCodes.Status400BadRequest;
                    problem.Title = "Invalid operation";
                    break;

                default:
                    problem.Status = StatusCodes.Status500InternalServerError;
                    problem.Title = "An error occurred while processing your request";
                    break;
            }

            httpContext.Response.ContentType = "application/problem+json";
            httpContext.Response.StatusCode = problem.Status.Value;

            await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

            return true;
        }
    }
}
