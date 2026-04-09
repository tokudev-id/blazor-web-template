using BlazorWebTemplate.Client.Services.BackEnd;

namespace BlazorWebTemplate.Tests.Backend;

public sealed class BackEndOptionsValidatorTests
{
    private readonly BackEndOptionsValidator _validator = new();

    [Fact]
    public void Validate_ReturnsSuccessForValidOptions()
    {
        var result = _validator.Validate(null, new BackEndOptions
        {
            BaseUrl = "https://localhost:7001/",
            RequestTimeoutSeconds = 20,
            RetryCount = 2,
            RetryDelayMilliseconds = 250
        });

        Assert.True(result.Succeeded);
    }

    [Fact]
    public void Validate_ReturnsFailureForInvalidTimeout()
    {
        var result = _validator.Validate(null, new BackEndOptions
        {
            BaseUrl = "https://localhost:7001/",
            RequestTimeoutSeconds = 1,
            RetryCount = 2,
            RetryDelayMilliseconds = 250
        });

        Assert.False(result.Succeeded);
    }

    [Fact]
    public void Validate_ReturnsFailureForZeroPageSize()
    {
        var result = _validator.Validate(null, new BackEndOptions
        {
            BaseUrl = "https://localhost:7001/",
            RequestTimeoutSeconds = 20,
            RetryCount = 2,
            RetryDelayMilliseconds = 250,
            DefaultPageSize = 0
        });

        Assert.False(result.Succeeded);
    }
}
