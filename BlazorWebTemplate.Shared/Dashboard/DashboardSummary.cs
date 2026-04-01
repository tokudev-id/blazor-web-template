using BlazorWebTemplate.Shared.Posts;

namespace BlazorWebTemplate.Shared.Dashboard;

public sealed record DashboardSummary(
    int TotalPosts,
    int TotalCategories,
    int TotalUsers,
    string MyRole,
    IReadOnlyList<PostSummary> RecentPosts,
    DateTimeOffset LastUpdatedUtc);
