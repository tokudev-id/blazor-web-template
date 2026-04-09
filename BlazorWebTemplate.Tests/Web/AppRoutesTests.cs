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

    [Theory]
    [InlineData("", 1, "/users")]
    [InlineData("", 2, "/users?page=2")]
    [InlineData("alice", 1, "/users?search=alice")]
    [InlineData("alice", 2, "/users?search=alice&page=2")]
    [InlineData("hello world", 3, "/users?search=hello%20world&page=3")]
    public void UsersWithSearchAndPage_BuildsExpectedUrl(string search, int page, string expected)
    {
        Assert.Equal(expected, RouteFor.WithSearchAndPage(search, page));
    }
}
