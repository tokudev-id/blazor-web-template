using BlazorWebTemplate.Client;
using BlazorWebTemplate.Client.Services.BackEnd;
using BlazorWebTemplate.Client.Services.BackEnd.Auth;
using BlazorWebTemplate.Client.Services.BackEnd.Categories;
using BlazorWebTemplate.Client.Services.BackEnd.Dashboard;
using BlazorWebTemplate.Client.Services.BackEnd.Posts;
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
                ["DummyJson:BaseUrl"] = "https://dummyjson.com/",
                ["DummyJson:RequestTimeoutSeconds"] = "20",
                ["DummyJson:RetryCount"] = "2",
                ["DummyJson:RetryDelayMilliseconds"] = "250"
            })
            .Build();

        services.AddLogging();
        services.AddHttpContextAccessor();
        services.AddAuthenticationCore();
        services.AddAuthorizationCore();
        services.AddClient(configuration);

        using var provider = services.BuildServiceProvider();

        Assert.Equal("https://dummyjson.com/", provider.GetRequiredService<IOptions<BackEndOptions>>().Value.BaseUrl);
        Assert.NotNull(provider.GetRequiredService<IAuthService>());
        Assert.NotNull(provider.GetRequiredService<IPostService>());
        Assert.NotNull(provider.GetRequiredService<ICategoryService>());
        Assert.NotNull(provider.GetRequiredService<IUserService>());
        Assert.NotNull(provider.GetRequiredService<IDashboardService>());
    }
}
