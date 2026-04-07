namespace BlazorWebTemplate.Client.Services.BackEnd;

public sealed class BackEndOptions
{
    public const string SectionName = "DummyJson";

    public string BaseUrl { get; set; } = "https://dummyjson.com/";

    public int RequestTimeoutSeconds { get; set; } = 20;

    public int RetryCount { get; set; } = 2;

    public int RetryDelayMilliseconds { get; set; } = 250;
}
