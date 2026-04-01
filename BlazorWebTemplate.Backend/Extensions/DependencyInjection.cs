using BlazorWebTemplate.Backend.Auth;
using BlazorWebTemplate.Backend.Categories;
using BlazorWebTemplate.Backend.Dashboard;
using BlazorWebTemplate.Backend.Http;
using BlazorWebTemplate.Backend.Mapping;
using BlazorWebTemplate.Backend.Posts;
using BlazorWebTemplate.Backend.Session;
using BlazorWebTemplate.Backend.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorWebTemplate.Backend.Extensions;

public static class ServiceRegistration
{
    public const string ApiClientName = "DummyJson.Api";
    public const string AuthClientName = "DummyJson.Auth";

    public static IServiceCollection AddBackendServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DummyJsonOptions>(configuration.GetSection(DummyJsonOptions.SectionName));
        services.AddHttpContextAccessor();

        services.AddScoped<IUserRoleMapper, UserRoleMapper>();
        services.AddScoped<ITokenStore, CookieAuthenticationTokenStore>();
        services.AddScoped<AccessTokenHandler>();
        services.AddScoped<TokenRefreshHandler>();

        services.AddHttpClient(AuthClientName, (serviceProvider, client) => ConfigureClient(serviceProvider, client));
        services.AddHttpClient(ApiClientName, (serviceProvider, client) => ConfigureClient(serviceProvider, client))
            .AddHttpMessageHandler<AccessTokenHandler>()
            .AddHttpMessageHandler<TokenRefreshHandler>();

        services.AddScoped<DummyJsonAuthApiClient>();
        services.AddScoped<DummyJsonPostsApiClient>();
        services.AddScoped<DummyJsonCategoriesApiClient>();
        services.AddScoped<DummyJsonUsersApiClient>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IDashboardService, DashboardService>();

        return services;
    }

    private static void ConfigureClient(IServiceProvider serviceProvider, HttpClient client)
    {
        var options = serviceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<DummyJsonOptions>>().Value;
        client.BaseAddress = new Uri(options.BaseUrl, UriKind.Absolute);
        client.Timeout = TimeSpan.FromSeconds(Math.Max(options.RequestTimeoutSeconds, 5));
    }
}
