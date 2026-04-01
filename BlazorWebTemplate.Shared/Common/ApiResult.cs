namespace BlazorWebTemplate.Shared.Common;

public class ApiResult
{
    protected ApiResult(bool isSuccess, ApiError? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public ApiError? Error { get; }

    public static ApiResult Success() => new(true, null);

    public static ApiResult Failure(ApiError error) => new(false, error);
}

public sealed class ApiResult<T> : ApiResult
{
    private ApiResult(T? value, bool isSuccess, ApiError? error)
        : base(isSuccess, error)
    {
        Value = value;
    }

    public T? Value { get; }

    public static ApiResult<T> Success(T value) => new(value, true, null);

    public new static ApiResult<T> Failure(ApiError error) => new(default, false, error);
}
