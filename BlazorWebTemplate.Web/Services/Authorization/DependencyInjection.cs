using BlazorWebTemplate.Shared.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorWebTemplate.Web.Services.Authorization;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthorizationServices(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AppPolicies.AdminOnly, policy => policy.RequireRole(RoleNames.Admin));
            options.AddPolicy(AppPolicies.EditorOrAbove, policy => policy.RequireRole(RoleNames.Admin, RoleNames.Editor));
        });

        return services;
    }
}
