using BlazorWebTemplate.Web.Common.Constants;
using BlazorWebTemplate.Web.Services.Shell;

namespace BlazorWebTemplate.Tests.Web;

public sealed class AppShellStateTests
{
    [Fact]
    public void ToggleDrawer_FlipsDrawerState()
    {
        var state = new AppShellState();
        var initial = state.IsDrawerOpen;

        state.ToggleDrawer();

        Assert.NotEqual(initial, state.IsDrawerOpen);
    }

    [Fact]
    public void SetPage_StoresTitleSectionAndBreadcrumbs()
    {
        var state = new AppShellState();

        state.SetPage("Posts", "Content", [new AppShellBreadcrumb("Dashboard", AppRoutes.Dashboard), new AppShellBreadcrumb("Posts", AppRoutes.Posts)]);

        Assert.Equal("Posts", state.CurrentTitle);
        Assert.Equal("Content", state.CurrentSection);
        Assert.Equal(2, state.Breadcrumbs.Count);
    }
}
