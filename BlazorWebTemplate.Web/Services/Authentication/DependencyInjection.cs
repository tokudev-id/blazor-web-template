using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorWebTemplate.Web.Services.Authentication;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<AuthenticationStateProvider, HttpContextAuthenticationStateProvider>();
        services.AddCascadingAuthenticationState();

        return services;
    }
}
