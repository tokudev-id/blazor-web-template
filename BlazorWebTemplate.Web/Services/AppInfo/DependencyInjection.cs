using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BlazorWebTemplate.Web.Services.AppInfo;

public static class DependencyInjection
{
    public static IServiceCollection AddAppInfoServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<AppBrandOptions>()
            .Bind(configuration.GetSection(AppBrandOptions.SectionName));
        services.AddSingleton<IValidateOptions<AppBrandOptions>, AppBrandOptionsValidator>();
        services.AddOptions<AppBrandOptions>().ValidateOnStart();

        return services;
    }
}
