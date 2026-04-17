using Performance.Application.Common.Enums;

namespace Performance.Application.Common.Models
{
    public class ResultError
    {
        public ErrorType ErrorType { get; init; }
        public required string Message { get; init; }
        public object? Payload { get; init; }
    }
}