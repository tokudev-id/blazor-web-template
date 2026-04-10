namespace BlazorWebTemplate.Shared.AuditLogs.Queries.GetAuditLogs;

public sealed record AuditLogSummary(
    string Id,
    string EntityName,
    string? EntityId,
    string Action,
    string? UserEmail,
    string? IPAddress,
    DateTimeOffset Timestamp,
    string? CorrelationId,
    string? OldValues = null,
    string? NewValues = null);
