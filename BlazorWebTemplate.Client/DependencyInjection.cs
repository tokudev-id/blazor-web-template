using BlazorWebTemplate.Client.Services.BackEnd;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorWebTemplate.Client;

public static class DependencyInjection
{
    public static IServiceCollection AddClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddBackEndServices(configuration);
        return services;
    }
}
