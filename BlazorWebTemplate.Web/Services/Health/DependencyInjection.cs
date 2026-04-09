using Microsoft.Extensions.DependencyInjection;

namespace BlazorWebTemplate.Web.Services.Health;

public static class DependencyInjection
{
    public static IServiceCollection AddHealthServices(this IServiceCollection services)
    {
        services.AddProblemDetails();
        services.AddHealthChecks();
        return services;
    }
}
