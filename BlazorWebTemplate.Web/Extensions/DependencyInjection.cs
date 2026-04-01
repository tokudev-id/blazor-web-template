using BlazorWebTemplate.Shared.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorWebTemplate.Web.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AppPolicies.AdminOnly, policy => policy.RequireRole(RoleNames.Admin));
            options.AddPolicy(AppPolicies.EditorOrAbove, policy => policy.RequireRole(RoleNames.Admin, RoleNames.Editor));
        });

        return services;
    }
}
