namespace Consumer.Domain.Utils;

public class Result
{
    public bool Success { get; set; }

    public static Result<T> CreateSuccess<T>(T result) => new() {Data = result, Success = true};
    
    public static Result<T> CreateFailure<T>() => new() {Success = false};
}

public class Result<T> : Result
{
    public T? Data { get; set; }
}