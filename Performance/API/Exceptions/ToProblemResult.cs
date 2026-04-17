using Microsoft.AspNetCore.Mvc;
using Performance.Application.Common.Enums;
using Performance.Application.Common.Models;

namespace Performance.API.Exceptions
{
    public static class ToProblemResult
    {
        public static ProblemDetails FromResultError(ResultError error)
        {
            var (statusCode, title) = error.ErrorType switch
            {
                ErrorType.BatchSizeExceeded => (StatusCodes.Status400BadRequest, "Batch Size Exceeded"),
                ErrorType.ValidationError => (StatusCodes.Status400BadRequest, "Validation Error"),
                ErrorType.NotFound => (StatusCodes.Status404NotFound, "Not Found"),
                ErrorType.Conflict => (StatusCodes.Status409Conflict, "Conflict"),
                _ => (StatusCodes.Status500InternalServerError, "An error occurred.")
            };

            return new ProblemDetails
            {
                Title = title,
                Status = statusCode,
                Detail = error.Message,
                Extensions =
                {
                    ["errors"] = error.Payload ?? Array.Empty<object>()
                }
            };
        }
    }
}