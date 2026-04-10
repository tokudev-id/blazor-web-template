using BlazorWebTemplate.Client.Services.BackEnd.Infrastructure.Http;
using BlazorWebTemplate.Shared.Common.Responses;
using Microsoft.Extensions.Logging;

namespace BlazorWebTemplate.Client.Services.BackEnd.AuditLogs;

internal sealed class AuditLogApi(
    IHttpClientFactory httpClientFactory,
    ILogger<AuditLogApi> logger) : BaseApiService(logger), IAuditLogApi
{
    public async Task<ApiResult<UnictiveAuditLogListDto>> GetLogsAsync(
        string? entityName = null,
        string? userId = null,
        string? action = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = new List<string>
        {
            $"pageNumber={pageNumber}",
            $"pageSize={pageSize}",
        };

        if (!string.IsNullOrWhiteSpace(entityName))
            query.Add($"entityName={Uri.EscapeDataString(entityName)}");
        if (!string.IsNullOrWhiteSpace(userId))
            query.Add($"userId={Uri.EscapeDataString(userId)}");
        if (!string.IsNullOrWhiteSpace(action))
            query.Add($"action={Uri.EscapeDataString(action)}");
        if (fromDate.HasValue)
            query.Add($"fromDate={fromDate.Value:O}");
        if (toDate.HasValue)
            query.Add($"toDate={toDate.Value:O}");

        var url = $"api/v1/auditlog?{string.Join('&', query)}";
        var client = httpClientFactory.CreateClient(DependencyInjection.ApiClientName);
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        return await SendForResultAsync<UnictiveAuditLogListDto>(client, request, cancellationToken);
    }
}
