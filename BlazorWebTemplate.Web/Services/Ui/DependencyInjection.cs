using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;

namespace BlazorWebTemplate.Web.Services.Ui;

public static class DependencyInjection
{
    public static IServiceCollection AddUiServices(this IServiceCollection services)
    {
        services.AddMudServices(configuration =>
        {
            configuration.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
            configuration.SnackbarConfiguration.PreventDuplicates = true;
            configuration.SnackbarConfiguration.NewestOnTop = true;
            configuration.SnackbarConfiguration.ShowCloseIcon = true;
        });

        return services;
    }
}
