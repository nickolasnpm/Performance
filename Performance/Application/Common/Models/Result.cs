namespace Performance.Application.Common.Models
{
    public class Result<TSuccess, TError>
    {
        public TSuccess? Data { get; init; }
        public TError? Error { get; init; }
        public bool IsSuccess { get; init; }

        public static Result<TSuccess, TError> Success(TSuccess data) =>
            new() { Data = data, IsSuccess = true };

        public static Result<TSuccess, TError> Failure(TError error) =>
            new() { Error = error, IsSuccess = false };
    }
}
