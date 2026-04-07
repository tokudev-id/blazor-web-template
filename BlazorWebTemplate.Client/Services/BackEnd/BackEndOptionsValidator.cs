using Microsoft.Extensions.Options;

namespace BlazorWebTemplate.Client.Services.BackEnd;

public sealed class BackEndOptionsValidator : IValidateOptions<BackEndOptions>
{
    public ValidateOptionsResult Validate(string? name, BackEndOptions options)
    {
        var failures = new List<string>();

        if (!Uri.TryCreate(options.BaseUrl, UriKind.Absolute, out _))
        {
            failures.Add("DummyJson:BaseUrl must be an absolute URI.");
        }

        if (options.RequestTimeoutSeconds is < 5 or > 120)
        {
            failures.Add("DummyJson:RequestTimeoutSeconds must be between 5 and 120 seconds.");
        }

        if (options.RetryCount is < 0 or > 5)
        {
            failures.Add("DummyJson:RetryCount must be between 0 and 5.");
        }

        if (options.RetryDelayMilliseconds is < 100 or > 5000)
        {
            failures.Add("DummyJson:RetryDelayMilliseconds must be between 100 and 5000 milliseconds.");
        }

        return failures.Count > 0 ? ValidateOptionsResult.Fail(failures) : ValidateOptionsResult.Success;
    }
}
