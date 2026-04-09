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

        state.SetPage("Users", "Administration", [new AppShellBreadcrumb("Dashboard", CommonRouteFor.Dashboard), new AppShellBreadcrumb("Users", "/users")]);

        Assert.Equal("Users", state.CurrentTitle);
        Assert.Equal("Administration", state.CurrentSection);
        Assert.Equal(2, state.Breadcrumbs.Count);
    }
}
