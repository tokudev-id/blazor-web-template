using BlazorWebTemplate.Web.Common.Pages.Dashboard;
using BlazorWebTemplate.Web.Features.Posts.State;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorWebTemplate.Web.Services.Shell;

public static class DependencyInjection
{
    public static IServiceCollection AddShellServices(this IServiceCollection services)
    {
        services.AddScoped<AppShellState>();
        services.AddScoped<DashboardPageState>();
        services.AddScoped<PostsPageState>();
        return services;
    }
}
