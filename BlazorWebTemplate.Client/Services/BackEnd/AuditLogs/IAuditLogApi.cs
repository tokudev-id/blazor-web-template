using BlazorWebTemplate.Shared.Common.Responses;

namespace BlazorWebTemplate.Client.Services.BackEnd.AuditLogs;

internal interface IAuditLogApi
{
    Task<ApiResult<UnictiveAuditLogListDto>> GetLogsAsync(
        string? entityName = null,
        string? userId = null,
        string? action = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default);
}
