using BlazorWebTemplate.Shared.Dashboard;

namespace BlazorWebTemplate.Web.Common.Pages.Dashboard;

public sealed class DashboardPageState
{
    private static readonly TimeSpan FreshnessWindow = TimeSpan.FromMinutes(2);

    private DashboardSummary? _summary;
    private DateTimeOffset? _loadedAtUtc;

    public bool TryGetFreshSummary(out DashboardSummary? summary)
    {
        if (_summary is not null && _loadedAtUtc is not null && DateTimeOffset.UtcNow - _loadedAtUtc <= FreshnessWindow)
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
