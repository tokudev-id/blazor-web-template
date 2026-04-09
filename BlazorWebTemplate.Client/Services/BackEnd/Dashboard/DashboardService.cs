using BlazorWebTemplate.Client.Services.BackEnd.Auth;
using BlazorWebTemplate.Client.Services.BackEnd.Users;
using BlazorWebTemplate.Shared.Common.Constants;
using BlazorWebTemplate.Shared.Common.Responses;
using BlazorWebTemplate.Shared.Dashboard.Queries.GetDashboard;

namespace BlazorWebTemplate.Client.Services.BackEnd.Dashboard;

internal sealed class DashboardService(
    IUserService userService,
    ICurrentUserService currentUserService) : IDashboardService
{
    public async Task<ApiResult<DashboardSummary>> GetSummaryAsync(CancellationToken cancellationToken = default)
    {
        var usersTask = userService.GetUsersAsync(cancellationToken: cancellationToken);
        var currentUserTask = currentUserService.GetCurrentUserAsync(cancellationToken);

        await Task.WhenAll(usersTask, currentUserTask);

        if (usersTask.Result.IsFailure)
        {
            return ApiResult<DashboardSummary>.Failure(usersTask.Result.Error ?? new ApiError(ApiErrorCodes.Unknown, "Unable to load the dashboard users."));
        }

        var paged = usersTask.Result.Value;
        var currentUser = currentUserTask.Result;
        var items = paged?.Items ?? [];

        var activeUsers = items.Count(u => u.IsActive);
        var inactiveUsers = items.Count(u => !u.IsActive);
        var roleDistribution = items
            .GroupBy(u => u.Role)
            .ToDictionary(g => g.Key, g => g.Count());

        return ApiResult<DashboardSummary>.Success(new DashboardSummary(
            paged?.TotalCount ?? 0,
            activeUsers,
            inactiveUsers,
            roleDistribution,
            currentUser?.Role ?? "Guest",
            DateTimeOffset.UtcNow));
    }
}
