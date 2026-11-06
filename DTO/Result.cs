public enum ErrorType
{
    NotFound,
    Validation,
    Conflict
}

public class Result<T>
{
    public T? Value { get; set; }
    public string? Error { get; set; }
    public ErrorType errorType { get; set; }
    public bool IsSuccess => Error == null;


}