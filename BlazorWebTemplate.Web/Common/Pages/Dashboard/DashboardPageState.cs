using BlazorWebTemplate.Shared.Dashboard.Queries.GetDashboard;
using Microsoft.Extensions.Options;

namespace BlazorWebTemplate.Web.Common.Pages.Dashboard;

public sealed class DashboardPageState(IOptions<AppDashboardOptions> dashboardOptions)
{
    private DashboardSummary? _summary;
    private DateTimeOffset? _loadedAtUtc;

    public bool TryGetFreshSummary(out DashboardSummary? summary)
    {
        var freshnessWindow = TimeSpan.FromMinutes(dashboardOptions.Value.FreshnessMinutes);
        if (_summary is not null && _loadedAtUtc is not null && DateTimeOffset.UtcNow - _loadedAtUtc <= freshnessWindow)
        {
            summary = _summary;
            return true;
        }

        summary = null;
        return false;
    }

    public void SetSummary(DashboardSummary summary)
    {
        _summary = summary;
        _loadedAtUtc = DateTimeOffset.UtcNow;
    }
}
