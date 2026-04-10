using BlazorWebTemplate.Shared.AuditLogs.Queries.GetAuditLogs;
using BlazorWebTemplate.Shared.Common.Constants;
using BlazorWebTemplate.Shared.Common.Responses;
using Microsoft.Extensions.Options;

namespace BlazorWebTemplate.Client.Services.BackEnd.AuditLogs;

internal sealed class AuditLogService(
    IAuditLogApi auditLogApi,
    IOptions<BackEndOptions> backEndOptions) : IAuditLogService
{
    public async Task<ApiResult<PagedResult<AuditLogSummary>>> GetLogsAsync(
        string? entityName = null,
        string? userId = null,
        string? action = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int pageNumber = 1,
        CancellationToken cancellationToken = default)
    {
        var pageSize = backEndOptions.Value.DefaultPageSize;
        var result = await auditLogApi.GetLogsAsync(entityName, userId, action, fromDate, toDate, pageNumber, pageSize, cancellationToken);

        if (result.IsFailure || result.Value is null)
            return ApiResult<PagedResult<AuditLogSummary>>.Failure(result.Error ?? new ApiError(ApiErrorCodes.Unknown, "Unable to load audit logs."));

        var items = result.Value.Items.Select(MapToSummary).ToList();
        var paged = new PagedResult<AuditLogSummary>(items, result.Value.PageNumber, result.Value.PageSize, result.Value.TotalCount);

        return ApiResult<PagedResult<AuditLogSummary>>.Success(paged);
    }

    private static AuditLogSummary MapToSummary(UnictiveAuditLogDto dto) => new(
        dto.Id.ToString(),
        dto.EntityName,
        dto.EntityId,
        dto.Action,
        dto.UserEmail,
        dto.IPAddress,
        new DateTimeOffset(DateTime.SpecifyKind(dto.Timestamp, DateTimeKind.Utc)),
        dto.CorrelationId,
        dto.OldValues,
        dto.NewValues);
}
