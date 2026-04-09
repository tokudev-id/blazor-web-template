using BlazorWebTemplate.Web.Common.Constants;
using BlazorWebTemplate.Web.Services.AppInfo;
using BlazorWebTemplate.Web.Services.Shell;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

namespace BlazorWebTemplate.Web.Layouts;

public partial class MainLayout : IDisposable
{
    [Inject]
    private AppShellState ShellState { get; set; } = default!;

    [Inject]
    private IOptions<AppBrandOptions> BrandOptionsAccessor { get; set; } = default!;

    protected AppBrandOptions Brand { get; private set; } = new();

    protected override void OnInitialized()
    {
        Brand = BrandOptionsAccessor.Value;
        ShellState.SetPage("Dashboard", "Workspace", CommonBreadcrumbFor.Dashboard());
        ShellState.Changed += HandleShellChanged;
    }

    private void HandleShellChanged()
        => InvokeAsync(StateHasChanged);

    public void Dispose()
    {
        ShellState.Changed -= HandleShellChanged;
    }
}
