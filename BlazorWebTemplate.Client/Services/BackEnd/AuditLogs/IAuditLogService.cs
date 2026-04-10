using BlazorWebTemplate.Shared.AuditLogs.Queries.GetAuditLogs;
using BlazorWebTemplate.Shared.Common.Responses;

namespace BlazorWebTemplate.Client.Services.BackEnd.AuditLogs;

public interface IAuditLogService
{
    Task<ApiResult<PagedResult<AuditLogSummary>>> GetLogsAsync(
        string? entityName = null,
        string? userId = null,
        string? action = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int pageNumber = 1,
        CancellationToken cancellationToken = default);
}
