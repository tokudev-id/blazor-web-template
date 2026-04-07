using BlazorWebTemplate.Client.Services.BackEnd.Auth;
using BlazorWebTemplate.Client.Services.BackEnd.Categories;
using BlazorWebTemplate.Client.Services.BackEnd.Dashboard;
using BlazorWebTemplate.Client.Services.BackEnd.Infrastructure.Mapping;
using BlazorWebTemplate.Client.Services.BackEnd.Infrastructure.Http;
using BlazorWebTemplate.Client.Services.BackEnd.Posts;
using BlazorWebTemplate.Client.Services.BackEnd.Infrastructure.Session;
using BlazorWebTemplate.Client.Services.BackEnd.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BlazorWebTemplate.Client.Services.BackEnd;

public static class DependencyInjection
{
    public const string ApiClientName = "DummyJson.Api";
    public const string AuthClientName = "DummyJson.Auth";

    public static IServiceCollection AddBackEndServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<BackEndOptions>()
            .Bind(configuration.GetSection(BackEndOptions.SectionName));
        services.AddSingleton<IValidateOptions<BackEndOptions>, BackEndOptionsValidator>();
        services.AddOptions<BackEndOptions>().ValidateOnStart();
        services.AddHttpContextAccessor();

        services.AddScoped<IUserRoleMapper, UserRoleMapper>();
        services.AddScoped<ITokenStore, CookieAuthenticationTokenStore>();
        services.AddScoped<AccessTokenHandler>();
        services.AddScoped<TokenRefreshHandler>();
        services.AddScoped<CorrelationIdHandler>();
        services.AddScoped<TransientHttpRetryHandler>();

        services.AddHttpClient(AuthClientName, (serviceProvider, client) => ConfigureClient(serviceProvider, client))
            .AddHttpMessageHandler<CorrelationIdHandler>()
            .AddHttpMessageHandler<TransientHttpRetryHandler>();
        services.AddHttpClient(ApiClientName, (serviceProvider, client) => ConfigureClient(serviceProvider, client))
            .AddHttpMessageHandler<CorrelationIdHandler>()
            .AddHttpMessageHandler<TransientHttpRetryHandler>()
            .AddHttpMessageHandler<AccessTokenHandler>()
            .AddHttpMessageHandler<TokenRefreshHandler>();

        services.AddScoped<IAuthApi, AuthApi>();
        services.AddScoped<IPostApi, PostApi>();
        services.AddScoped<ICategoryApi, CategoryApi>();
        services.AddScoped<IUserApi, UserApi>();

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
        var options = serviceProvider.GetRequiredService<IOptions<BackEndOptions>>().Value;
        client.BaseAddress = new Uri(options.BaseUrl, UriKind.Absolute);
        client.Timeout = TimeSpan.FromSeconds(Math.Max(options.RequestTimeoutSeconds, 5));
    }
}
