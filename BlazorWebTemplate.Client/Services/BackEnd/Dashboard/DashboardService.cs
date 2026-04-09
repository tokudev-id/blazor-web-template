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

        var users = usersTask.Result.Value ?? [];
        var currentUser = currentUserTask.Result;

        return ApiResult<DashboardSummary>.Success(new DashboardSummary(
            users.Count,
            currentUser?.Role ?? "Guest",
            DateTimeOffset.UtcNow));
    }
}
