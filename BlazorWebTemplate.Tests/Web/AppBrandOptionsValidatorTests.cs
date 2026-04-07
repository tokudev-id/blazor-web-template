using BlazorWebTemplate.Web.Services.AppInfo;

namespace BlazorWebTemplate.Tests.Web;

public sealed class AppBrandOptionsValidatorTests
{
    private readonly AppBrandOptionsValidator _validator = new();

    [Fact]
    public void Validate_ReturnsSuccessForValidBranding()
    {
        var result = _validator.Validate(null, new AppBrandOptions
        {
            ProductName = "Control Center",
            ProductTagline = "Enterprise operations",
            ProductDescription = "A professional admin shell for workflow and content operations.",
            CompanyName = "Unictive",
            SupportEmail = "support@example.com",
            PrimaryColor = "#1F4FD8",
            SecondaryColor = "#0F172A",
            AccentColor = "#14B8A6"
        });

        Assert.True(result.Succeeded);
    }

    [Fact]
    public void Validate_ReturnsFailureForInvalidPrimaryColor()
    {
        var result = _validator.Validate(null, new AppBrandOptions
        {
            ProductName = "Control Center",
            ProductTagline = "Enterprise operations",
            ProductDescription = "A professional admin shell for workflow and content operations.",
            CompanyName = "Unictive",
            SupportEmail = "support@example.com",
            PrimaryColor = "blue",
            SecondaryColor = "#0F172A",
            AccentColor = "#14B8A6"
        });

        Assert.False(result.Succeeded);
    }
}
