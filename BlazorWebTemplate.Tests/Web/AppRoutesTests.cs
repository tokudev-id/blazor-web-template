using BlazorWebTemplate.Web.Common.Constants;
using BlazorWebTemplate.Web.Features.Users.Constants;

namespace BlazorWebTemplate.Tests.Web;

public sealed class AppRoutesTests
{
    [Fact]
    public void LoginWithReturnUrl_BuildsExpectedQueryString()
    {
        var route = CommonRouteFor.LoginWithReturnUrl("/dashboard", "Nope");

        Assert.Equal("/login?returnUrl=%2Fdashboard&error=Nope", route);
    }

    [Fact]
    public void UsersSearch_BuildsStableUrl()
    {
        Assert.Equal("/users?search=alice", RouteFor.WithSearch("alice"));
        Assert.Equal("/users", RouteFor.WithSearch(string.Empty));
    }
}
