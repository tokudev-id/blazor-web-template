using BlazorWebTemplate.Shared.Dashboard;
using BlazorWebTemplate.Web.Common.Pages.Dashboard;

namespace BlazorWebTemplate.Tests.Web;

public sealed class DashboardPageStateTests
{
    [Fact]
    public void TryGetFreshSummary_ReturnsFalse_WhenEmpty()
    {
        var state = new DashboardPageState();

        var found = state.TryGetFreshSummary(out var summary);

        Assert.False(found);
        Assert.Null(summary);
    }

    [Fact]
    public void SetSummary_MakesSummaryAvailable()
    {
        var state = new DashboardPageState();
        var summary = new DashboardSummary(10, 4, 2, "Admin", [], DateTimeOffset.UtcNow);

        state.SetSummary(summary);

        Assert.True(state.TryGetFreshSummary(out var cached));
        Assert.Equal(summary, cached);
    }
}
