namespace BlazorWebTemplate.Client.Services.BackEnd;

public sealed class BackEndOptions
{
    public const string SectionName = "UnictiveApi";

    public string BaseUrl { get; set; } = "https://localhost:7001/";

    public int RequestTimeoutSeconds { get; set; } = 20;

    public int RetryCount { get; set; } = 2;

    public int RetryDelayMilliseconds { get; set; } = 250;

    public int DefaultPageSize { get; set; } = 20;
}
