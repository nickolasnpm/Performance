namespace Performance.API.Exceptions
{
    public class FieldValidationError
    {
        public string Field { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}