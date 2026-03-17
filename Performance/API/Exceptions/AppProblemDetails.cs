using Microsoft.AspNetCore.Mvc;

namespace Performance.API.Exceptions
{
    public class AppProblemDetails : ProblemDetails
    {
        public List<FieldError> Errors { get; set; } = [];
        public string? TraceId { get; set; }
    }
}