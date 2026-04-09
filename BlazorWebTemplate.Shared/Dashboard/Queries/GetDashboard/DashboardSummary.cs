namespace BlazorWebTemplate.Shared.Dashboard.Queries.GetDashboard;

public sealed record DashboardSummary(
    int TotalUsers,
    int ActiveUsers,
    int InactiveUsers,
    IReadOnlyDictionary<string, int> RoleDistribution,
    string MyRole,
    DateTimeOffset LastUpdatedUtc);
