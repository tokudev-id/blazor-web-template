using BlazorWebTemplate.Shared.Services.Authorization.Constants;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorWebTemplate.Web.Services.Authorization;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthorizationServices(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AppPolicyFor.AdminOnly, policy => policy.RequireRole(RoleNameFor.Admin));
            options.AddPolicy(AppPolicyFor.EditorOrAbove, policy => policy.RequireRole(RoleNameFor.Admin, RoleNameFor.Editor));
        });

        return services;
    }
}
