using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BlazorWebTemplate.Web.Services.AppInfo;
using BlazorWebTemplate.Web.Services.Authentication;
using BlazorWebTemplate.Web.Services.Authorization;
using BlazorWebTemplate.Web.Services.FrontEnd;
using BlazorWebTemplate.Web.Services.Health;
using BlazorWebTemplate.Web.Services.Notifications;
using BlazorWebTemplate.Web.Services.Shell;
using BlazorWebTemplate.Web.Services.Ui;

namespace BlazorWebTemplate.Web.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddBlazorWebTemplateWeb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAppInfoServices(configuration);
        services.AddAuthenticationServices(configuration);
        services.AddAuthorizationServices();
        services.AddFrontEndServices();
        services.AddHealthServices();
        services.AddNotificationServices();
        services.AddShellServices(configuration);
        services.AddUiServices();
        return services;
    }
}
