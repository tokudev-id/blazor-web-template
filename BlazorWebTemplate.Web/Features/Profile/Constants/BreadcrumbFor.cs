using BlazorWebTemplate.Web.Services.Shell;

namespace BlazorWebTemplate.Web.Features.Profile.Constants;

public static class BreadcrumbFor
{
    public static IReadOnlyList<AppShellBreadcrumb> Index() =>
    [
        new AppShellBreadcrumb("My profile", RouteFor.Index),
    ];

    public static IReadOnlyList<AppShellBreadcrumb> ChangePassword() =>
    [
        new AppShellBreadcrumb("Change Password", "/change-password"),
    ];
}
