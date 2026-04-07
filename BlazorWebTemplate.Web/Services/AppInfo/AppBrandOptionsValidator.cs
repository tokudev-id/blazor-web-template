using Microsoft.Extensions.Options;

namespace BlazorWebTemplate.Web.Services.AppInfo;

public sealed class AppBrandOptionsValidator : IValidateOptions<AppBrandOptions>
{
    public ValidateOptionsResult Validate(string? name, AppBrandOptions options)
    {
        var failures = new List<string>();

        if (string.IsNullOrWhiteSpace(options.ProductName))
        {
            failures.Add("Branding:ProductName is required.");
        }

        if (string.IsNullOrWhiteSpace(options.ProductTagline))
        {
            failures.Add("Branding:ProductTagline is required.");
        }

        if (string.IsNullOrWhiteSpace(options.ProductDescription))
        {
            failures.Add("Branding:ProductDescription is required.");
        }

        if (string.IsNullOrWhiteSpace(options.CompanyName))
        {
            failures.Add("Branding:CompanyName is required.");
        }

        if (string.IsNullOrWhiteSpace(options.SupportEmail) || !options.SupportEmail.Contains('@', StringComparison.Ordinal))
        {
            failures.Add("Branding:SupportEmail must be a valid email address.");
        }

        if (!IsHexColor(options.PrimaryColor))
        {
            failures.Add("Branding:PrimaryColor must be a 6-digit hex color.");
        }

        if (!IsHexColor(options.SecondaryColor))
        {
            failures.Add("Branding:SecondaryColor must be a 6-digit hex color.");
        }

        if (!IsHexColor(options.AccentColor))
        {
            failures.Add("Branding:AccentColor must be a 6-digit hex color.");
        }

        return failures.Count > 0 ? ValidateOptionsResult.Fail(failures) : ValidateOptionsResult.Success;
    }

    private static bool IsHexColor(string? value)
        => !string.IsNullOrWhiteSpace(value)
           && value.Length == 7
           && value[0] == '#'
           && value[1..].All(Uri.IsHexDigit);
}
