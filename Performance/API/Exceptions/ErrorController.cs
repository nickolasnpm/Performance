using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Performance.API.Exceptions
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController(ILogger<ErrorController> logger) : ControllerBase
    {
        [Route("validation-error")]
        public IActionResult HandleValidationError()
        {
            var modelState = HttpContext.Items["ModelState"] as ModelStateDictionary;
            var instance = HttpContext.Items["Instance"] as string;

            var errors = modelState?
                .Where(e => e.Value?.Errors.Count > 0)
                .SelectMany(kvp => kvp.Value!.Errors.Select(err => new FieldValidationError
                {
                    Field = kvp.Key,
                    Message = err.ErrorMessage
                }))
                .ToList() ?? new();

            return BadRequest(new ProblemDetails
            {
                Title = "Request validation failed",
                Status = StatusCodes.Status400BadRequest,
                Detail = $"{errors.Count} validation error(s) occurred.",
                Instance = instance,
                Extensions =
                {
                    ["traceId"] = HttpContext.TraceIdentifier,
                    ["errors"] = errors
                }
            });
        }

        [Route("/error")]
        public IActionResult HandleException()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            var exceptionType = $"{exceptionFeature?.Error.GetType().FullName}";
            var exceptionMessage = exceptionFeature?.Error?.Message ?? "No exception message available.";
            var exceptionInstance = $"{Request.Method} {exceptionFeature?.Path ?? Request.Path}";

            var (statusCode, title) = exceptionType switch
            {
                "System.ArgumentException" => (StatusCodes.Status400BadRequest, "Invalid request."),
                "System.InvalidOperationException" => (StatusCodes.Status400BadRequest, "Invalid operation."),
                "System.UnauthorizedAccessException" => (StatusCodes.Status401Unauthorized, "Unauthorized."),
                "System.KeyNotFoundException" => (StatusCodes.Status404NotFound, "Resource not found."),
                "System.NotSupportedException" => (StatusCodes.Status405MethodNotAllowed, "Method not allowed."),
                _ => (StatusCodes.Status500InternalServerError, "An error occurred.")
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