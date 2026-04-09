using BlazorWebTemplate.Client.Services.BackEnd.Dashboard;
using BlazorWebTemplate.Shared.Dashboard.Queries.GetDashboard;
using BlazorWebTemplate.Web.Common.Constants;
using BlazorWebTemplate.Web.Services.Shell;
using Microsoft.AspNetCore.Components;

namespace BlazorWebTemplate.Web.Common.Pages.Dashboard;

public partial class Dashboard
{
    [Inject]
    private IDashboardService DashboardService { get; set; } = default!;

    [Inject]
    private DashboardPageState DashboardState { get; set; } = default!;

    protected IReadOnlyList<AppShellBreadcrumb> _breadcrumbs = CommonBreadcrumbFor.Dashboard();
    protected DashboardSummary? _summary;
    protected bool _isLoading = true;
    protected string? _errorMessage;

    protected override async Task OnInitializedAsync()
    {
        if (DashboardState.TryGetFreshSummary(out var cachedSummary))
        {
            _summary = cachedSummary;
            _isLoading = false;
            return;
        }

        var result = await DashboardService.GetSummaryAsync();
        _isLoading = false;

        if (result.IsFailure)
        {
            _errorMessage = result.Error?.Message ?? "The dashboard summary could not be loaded.";
            return;
        }

        _summary = result.Value;
        if (_summary is not null)
        {
            DashboardState.SetSummary(_summary);
        }
    }
}
