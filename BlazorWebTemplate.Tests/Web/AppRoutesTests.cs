using BlazorWebTemplate.Web.Common.Constants;

namespace BlazorWebTemplate.Tests.Web;

public sealed class AppRoutesTests
{
    [Fact]
    public void LoginWithReturnUrl_BuildsExpectedQueryString()
    {
        var route = AppRoutes.LoginWithReturnUrl("/posts/42", "Nope");

        Assert.Equal("/login?returnUrl=%2Fposts%2F42&error=Nope", route);
    }

    [Fact]
    public void PostHelpers_BuildStableUrls()
    {
        Assert.Equal("/posts/42", AppRoutes.PostDetails(42));
        Assert.Equal("/posts/42/edit", AppRoutes.PostEdit(42));
        Assert.Equal("/posts/42/delete", AppRoutes.PostDelete(42));
    }
}
