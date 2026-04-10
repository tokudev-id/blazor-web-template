using BlazorWebTemplate.Web.Services.Shell;

namespace BlazorWebTemplate.Web.Features.AuditLogs.Constants;

public static class BreadcrumbFor
{
    public static IReadOnlyList<AppShellBreadcrumb> Index() =>
    [
        new AppShellBreadcrumb("Audit logs", RouteFor.Index),
    ];
}
