using BlazorWebTemplate.Client;
using BlazorWebTemplate.Client.Services.BackEnd;
using BlazorWebTemplate.Client.Services.BackEnd.Auth;
using BlazorWebTemplate.Client.Services.BackEnd.Dashboard;
using BlazorWebTemplate.Client.Services.BackEnd.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BlazorWebTemplate.Tests.Client;

public sealed class ClientDependencyInjectionTests
{
    [Fact]
    public void AddClient_RegistersBackEndServicesAndOptions()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["UnictiveApi:BaseUrl"] = "https://localhost:7001/",
                ["UnictiveApi:RequestTimeoutSeconds"] = "20",
                ["UnictiveApi:RetryCount"] = "2",
                ["UnictiveApi:RetryDelayMilliseconds"] = "250"
            })
            .Build();

        services.AddLogging();
        services.AddHttpContextAccessor();
        services.AddAuthenticationCore();
        services.AddAuthorizationCore();
        services.AddClient(configuration);

        using var provider = services.BuildServiceProvider();

        Assert.Equal("https://localhost:7001/", provider.GetRequiredService<IOptions<BackEndOptions>>().Value.BaseUrl);
        Assert.NotNull(provider.GetRequiredService<IAuthService>());
        Assert.NotNull(provider.GetRequiredService<IUserService>());
        Assert.NotNull(provider.GetRequiredService<IDashboardService>());
    }
}
