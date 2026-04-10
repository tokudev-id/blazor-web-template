namespace BlazorWebTemplate.Client.Services.BackEnd.AuditLogs;

internal sealed class UnictiveAuditLogListDto
{
    public List<UnictiveAuditLogDto> Items { get; set; } = [];
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
}

internal sealed class UnictiveAuditLogDto
{
    public Guid Id { get; set; }
    public string EntityName { get; set; } = string.Empty;
    public string? EntityId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public string? UserId { get; set; }
    public string? UserEmail { get; set; }
    public string? IPAddress { get; set; }
    public DateTime Timestamp { get; set; }
    public string? CorrelationId { get; set; }
}
