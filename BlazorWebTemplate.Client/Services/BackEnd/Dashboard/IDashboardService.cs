using BlazorWebTemplate.Shared.Common.Responses;
using BlazorWebTemplate.Shared.Dashboard.Queries.GetDashboard;

namespace BlazorWebTemplate.Client.Services.BackEnd.Dashboard;

public interface IDashboardService
{
    Task<ApiResult<DashboardSummary>> GetSummaryAsync(CancellationToken cancellationToken = default);
}
