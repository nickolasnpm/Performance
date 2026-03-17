using Microsoft.AspNetCore.Mvc;

namespace Performance.API.Exceptions
{
    public static class ValidationResponseFactory
    {
        public static IActionResult Create(ActionContext context, ILogger logger)
        {
            var errors = context.ModelState
                .Where(kv => kv.Value?.Errors.Count > 0)
                .SelectMany(kv => kv.Value!.Errors.Select(e => new FieldError
                {
                    Field = kv.Key,
                    Message = e.ErrorMessage
                }))
                .ToList();

            logger.LogError(
                "Validation failed. TraceId: {TraceId} | Timestamp: {Timestamp} | Method: {Method} | Path: {Path} | Errors: {@Errors}",
                context.HttpContext.TraceIdentifier,
                DateTime.UtcNow,
                context.HttpContext.Request.Method,
                context.HttpContext.Request.Path,
                errors);

            var problem = new AppProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Request validation failed",
                Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}",
                Detail = $"{errors.Count} validation error(s) occurred.",
                Errors = errors,
                TraceId = context.HttpContext.TraceIdentifier
            };

            return new BadRequestObjectResult(problem);
        }
    }
}