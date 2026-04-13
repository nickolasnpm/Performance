namespace Performance.Application.Common.Models
{
    public class ResultError
    {
        public int Code { get; init; }
        public required string Message { get; init; }
    }

    public class ResultError<TPayload> : ResultError
    {
        public required TPayload Payload { get; init; }
    }
}