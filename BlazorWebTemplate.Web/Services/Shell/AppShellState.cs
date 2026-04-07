using MudBlazor;
using BlazorWebTemplate.Web.Common.Constants;

namespace BlazorWebTemplate.Web.Services.Shell;

public sealed class AppShellState
{
    private List<AppShellBreadcrumb> _breadcrumbs = [new("Dashboard", AppRoutes.Dashboard)];

    public bool IsDrawerOpen { get; private set; } = true;

    public string CurrentTitle { get; private set; } = "Dashboard";

    public string? CurrentSection { get; private set; } = "Workspace";

    public IReadOnlyList<AppShellBreadcrumb> Breadcrumbs => _breadcrumbs;

    public event Action? Changed;

    public event Func<AppShellNotification, Task>? NotificationRaised;

    public void ToggleDrawer()
    {
        IsDrawerOpen = !IsDrawerOpen;
        Changed?.Invoke();
    }

    public void SetDrawer(bool isOpen)
    {
        if (IsDrawerOpen == isOpen)
        {
            return;
        }

        IsDrawerOpen = isOpen;
        Changed?.Invoke();
    }

    public void SetPage(string title, string? section = null, IEnumerable<AppShellBreadcrumb>? breadcrumbs = null)
    {
        CurrentTitle = string.IsNullOrWhiteSpace(title) ? "Workspace" : title;
        CurrentSection = string.IsNullOrWhiteSpace(section) ? "Workspace" : section;
        _breadcrumbs = breadcrumbs?.ToList() ?? [new AppShellBreadcrumb(CurrentTitle)];
        Changed?.Invoke();
    }

    public Task NotifyAsync(AppShellNotification notification)
        => NotificationRaised?.Invoke(notification) ?? Task.CompletedTask;
}

public sealed record AppShellBreadcrumb(string Label, string? Href = null);

public sealed record AppShellNotification(Severity Severity, string Message, string? Key = null);
