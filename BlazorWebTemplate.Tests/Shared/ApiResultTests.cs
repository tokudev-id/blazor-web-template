using BlazorWebTemplate.Shared.Common;

namespace BlazorWebTemplate.Tests.Shared;

public sealed class ApiResultTests
{
    [Fact]
    public void SuccessResult_SetsSuccessState()
    {
        var result = ApiResult.Success();

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Null(result.Error);
    }

    [Fact]
    public void FailureResult_SetsFailureState()
    {
        var error = new ApiError(ApiErrorCodes.Validation, "Validation failed");
        var result = ApiResult<string>.Failure(error);

        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(error, result.Error);
        Assert.Null(result.Value);
    }
}
