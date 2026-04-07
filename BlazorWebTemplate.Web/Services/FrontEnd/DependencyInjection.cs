using Microsoft.Extensions.DependencyInjection;

namespace BlazorWebTemplate.Web.Services.FrontEnd;

public static class DependencyInjection
{
    public static IServiceCollection AddFrontEndServices(this IServiceCollection services)
    {
        services.AddRazorComponents()
            .AddInteractiveServerComponents();

        return services;
    }
}
