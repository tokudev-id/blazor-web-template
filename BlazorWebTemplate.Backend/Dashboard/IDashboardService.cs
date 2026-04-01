using BlazorWebTemplate.Shared.Common;
using BlazorWebTemplate.Shared.Dashboard;

namespace BlazorWebTemplate.Backend.Dashboard;

public interface IDashboardService
{
    Task<ApiResult<DashboardSummary>> GetSummaryAsync(CancellationToken cancellationToken = default);
}
