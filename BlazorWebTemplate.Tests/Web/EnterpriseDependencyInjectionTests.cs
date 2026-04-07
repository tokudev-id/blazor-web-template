using BlazorWebTemplate.Shared.Auth;
using BlazorWebTemplate.Web.Services;
using BlazorWebTemplate.Web.Services.AppInfo;
using BlazorWebTemplate.Web.Services.Shell;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BlazorWebTemplate.Tests.Web;

public sealed class EnterpriseDependencyInjectionTests
{
    [Fact]
    public async Task AddBlazorWebTemplateWeb_RegistersPoliciesAndShellState()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Branding:ProductName"] = "Control Center",
                ["Branding:ProductTagline"] = "Enterprise operations",
                ["Branding:ProductDescription"] = "A professional admin shell for workflow and content operations.",
                ["Branding:CompanyName"] = "Unictive",
                ["Branding:SupportEmail"] = "support@example.com",
                ["Branding:PrimaryColor"] = "#1F4FD8",
                ["Branding:SecondaryColor"] = "#0F172A",
                ["Branding:AccentColor"] = "#14B8A6"
            })
            .Build();

        services.AddLogging();
        services.AddAuthorizationCore();
        services.AddBlazorWebTemplateWeb(configuration);

        await using var provider = services.BuildServiceProvider();

        var brandOptions = provider.GetRequiredService<IOptions<AppBrandOptions>>().Value;
        var shellState = provider.GetRequiredService<AppShellState>();
        var policyProvider = provider.GetRequiredService<IAuthorizationPolicyProvider>();
        var adminPolicy = await policyProvider.GetPolicyAsync(AppPolicies.AdminOnly);

        Assert.Equal("Control Center", brandOptions.ProductName);
        Assert.NotNull(shellState);
        Assert.NotNull(adminPolicy);
    }
}
