using BlazorWebTemplate.Web.Services.Shell;

namespace BlazorWebTemplate.Web.Common.Constants;

public static class AppBreadcrumbs
{
    public static IReadOnlyList<AppShellBreadcrumb> Dashboard()
        => [new("Dashboard", AppRoutes.Dashboard)];

    public static IReadOnlyList<AppShellBreadcrumb> Posts()
        => [new("Dashboard", AppRoutes.Dashboard), new("Posts", AppRoutes.Posts)];

    public static IReadOnlyList<AppShellBreadcrumb> PostDetails(string title, int id)
        => [new("Dashboard", AppRoutes.Dashboard), new("Posts", AppRoutes.Posts), new(title, AppRoutes.PostDetails(id))];

    public static IReadOnlyList<AppShellBreadcrumb> PostEditor(bool isEditMode)
        => [new("Dashboard", AppRoutes.Dashboard), new("Posts", AppRoutes.Posts), new(isEditMode ? "Edit post" : "Create post")];

    public static IReadOnlyList<AppShellBreadcrumb> PostDelete()
        => [new("Dashboard", AppRoutes.Dashboard), new("Posts", AppRoutes.Posts), new("Delete post")];

    public static IReadOnlyList<AppShellBreadcrumb> Categories()
        => [new("Dashboard", AppRoutes.Dashboard), new("Categories", AppRoutes.Categories)];

    public static IReadOnlyList<AppShellBreadcrumb> Users()
        => [new("Dashboard", AppRoutes.Dashboard), new("Users", AppRoutes.Users)];
}
