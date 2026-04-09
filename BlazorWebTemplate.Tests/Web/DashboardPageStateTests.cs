using BlazorWebTemplate.Shared.Dashboard.Queries.GetDashboard;
using BlazorWebTemplate.Web.Common.Pages.Dashboard;
using Microsoft.Extensions.Options;

namespace BlazorWebTemplate.Tests.Web;

public sealed class DashboardPageStateTests
{
    private static DashboardPageState CreateState(int freshnessMinutes = 2)
        => new(Options.Create(new AppDashboardOptions { FreshnessMinutes = freshnessMinutes }));

    [Fact]
    public void TryGetFreshSummary_ReturnsFalse_WhenEmpty()
    {
        var state = CreateState();

        var found = state.TryGetFreshSummary(out var summary);

        Assert.False(found);
        Assert.Null(summary);
    }

    [Fact]
    public void SetSummary_MakesSummaryAvailable()
    {
        var state = CreateState();
        var summary = new DashboardSummary(2, 1, 1, new Dictionary<string, int>(), "Admin", DateTimeOffset.UtcNow);

        state.SetSummary(summary);

        Assert.True(state.TryGetFreshSummary(out var cached));
        Assert.Equal(summary, cached);
    }
}
