using BlazorWebTemplate.Web.Common.Constants;
using BlazorWebTemplate.Web.Services.Shell;

namespace BlazorWebTemplate.Web.Features.Rbac.Constants;

public static class BreadcrumbFor
{
    public static IReadOnlyList<AppShellBreadcrumb> Index()
        => [new("Dashboard", CommonRouteFor.Dashboard), new("Roles", RouteFor.Index)];
}
