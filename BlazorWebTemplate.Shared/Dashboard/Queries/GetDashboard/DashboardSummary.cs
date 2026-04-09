namespace BlazorWebTemplate.Shared.Dashboard.Queries.GetDashboard;

public sealed record DashboardSummary(
    int TotalUsers,
    string MyRole,
    DateTimeOffset LastUpdatedUtc);
