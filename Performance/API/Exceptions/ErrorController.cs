using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Performance.API.Exceptions
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController(ILogger<ErrorController> logger) : ControllerBase
    {
        [Route("/error")]
        public IActionResult HandleException()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            var exceptionMessage = exceptionFeature?.Error?.Message ?? "No exception message available.";
            var exceptionInstance = $"{Request.Method} {exceptionFeature?.Path ?? Request.Path}";

            var (statusCode, title) = exceptionFeature?.Error switch
            {
                ArgumentException           => (StatusCodes.Status400BadRequest,          "Invalid request."),
                InvalidOperationException   => (StatusCodes.Status400BadRequest,          "Invalid operation."),
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized,        "Unauthorized."),
                KeyNotFoundException        => (StatusCodes.Status404NotFound,            "Resource not found."),
                NotSupportedException       => (StatusCodes.Status405MethodNotAllowed,    "Method not allowed."),
                _                           => (StatusCodes.Status500InternalServerError, "An error occurred.")
            };

            logger.LogError(
                exceptionFeature?.Error, 
                "A {StatusCode} error has been occured for TraceId: {TraceId} at Timestamp: {Timestamp}",
                statusCode,  
                HttpContext.TraceIdentifier,
                DateTime.UtcNow.ToLocalTime());

            return StatusCode(statusCode, new ProblemDetails
            {
                Title = title,
                Status = statusCode,
                Detail = exceptionMessage,
                Instance = exceptionInstance,
                Extensions =
                {
                    ["traceId"] = HttpContext.TraceIdentifier,
                    ["errors"] = Array.Empty<object>()
                }
            });
        }
    }
}