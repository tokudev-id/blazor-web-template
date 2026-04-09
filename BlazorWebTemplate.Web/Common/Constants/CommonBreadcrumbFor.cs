using BlazorWebTemplate.Web.Services.Shell;

namespace BlazorWebTemplate.Web.Common.Constants;

public static class CommonBreadcrumbFor
{
    public static IReadOnlyList<AppShellBreadcrumb> Dashboard()
        => [new("Dashboard", CommonRouteFor.Dashboard)];
}
