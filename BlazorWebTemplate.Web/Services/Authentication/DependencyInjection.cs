using BlazorWebTemplate.Shared.Services.Authentication.Constants;
using BlazorWebTemplate.Web.Common.Constants;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorWebTemplate.Web.Services.Authentication;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<AppAuthOptions>()
            .Bind(configuration.GetSection(AppAuthOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var authOptions = configuration.GetSection(AppAuthOptions.SectionName).Get<AppAuthOptions>() ?? new AppAuthOptions();

        services.AddAuthentication(AuthConstants.CookieScheme)
            .AddCookie(AuthConstants.CookieScheme, options =>
            {
                options.Cookie.Name = authOptions.CookieName;
                options.LoginPath = CommonRouteFor.Login;
                options.AccessDeniedPath = CommonRouteFor.Login;
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromHours(authOptions.ExpireTimeSpanHours);
            });

        services.AddHttpContextAccessor();
        services.AddScoped<AuthenticationStateProvider, HttpContextAuthenticationStateProvider>();
        services.AddCascadingAuthenticationState();

        return services;
    }
}
