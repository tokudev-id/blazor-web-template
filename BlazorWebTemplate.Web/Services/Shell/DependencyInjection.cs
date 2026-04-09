using BlazorWebTemplate.Web.Common.Pages.Dashboard;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorWebTemplate.Web.Services.Shell;

public static class DependencyInjection
{
    public static IServiceCollection AddShellServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<AppDashboardOptions>()
            .Bind(configuration.GetSection(AppDashboardOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped<AppShellState>();
        services.AddScoped<DashboardPageState>();
        return services;
    }
}
