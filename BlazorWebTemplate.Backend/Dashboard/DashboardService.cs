using BlazorWebTemplate.Backend.Auth;
using BlazorWebTemplate.Backend.Categories;
using BlazorWebTemplate.Backend.Posts;
using BlazorWebTemplate.Backend.Users;
using BlazorWebTemplate.Shared.Common;
using BlazorWebTemplate.Shared.Dashboard;
using BlazorWebTemplate.Shared.Posts;

namespace BlazorWebTemplate.Backend.Dashboard;

internal sealed class DashboardService(
    IPostService postService,
    ICategoryService categoryService,
    IUserService userService,
    ICurrentUserService currentUserService) : IDashboardService
{
    public async Task<ApiResult<DashboardSummary>> GetSummaryAsync(CancellationToken cancellationToken = default)
    {
        var postsTask = postService.GetPostsAsync(new PostQuery(PageNumber: 1, PageSize: 5), cancellationToken);
        var categoriesTask = categoryService.GetCategoriesAsync(cancellationToken: cancellationToken);
        var usersTask = userService.GetUsersAsync(cancellationToken: cancellationToken);
        var currentUserTask = currentUserService.GetCurrentUserAsync(cancellationToken);

        await Task.WhenAll(postsTask, categoriesTask, usersTask, currentUserTask);

        if (postsTask.Result.IsFailure)
        {
            return ApiResult<DashboardSummary>.Failure(postsTask.Result.Error ?? new ApiError(ApiErrorCodes.Unknown, "Unable to load the dashboard posts."));
        }

        if (categoriesTask.Result.IsFailure)
        {
            return ApiResult<DashboardSummary>.Failure(categoriesTask.Result.Error ?? new ApiError(ApiErrorCodes.Unknown, "Unable to load the dashboard categories."));
        }

        if (usersTask.Result.IsFailure)
        {
            return ApiResult<DashboardSummary>.Failure(usersTask.Result.Error ?? new ApiError(ApiErrorCodes.Unknown, "Unable to load the dashboard users."));
        }

        var posts = postsTask.Result.Value?.Items ?? [];
        var categories = categoriesTask.Result.Value ?? [];
        var users = usersTask.Result.Value ?? [];
        var currentUser = currentUserTask.Result;

        return ApiResult<DashboardSummary>.Success(new DashboardSummary(
            postsTask.Result.Value?.TotalCount ?? posts.Count,
            categories.Count,
            users.Count,
            currentUser?.Role ?? "Guest",
            posts,
            DateTimeOffset.UtcNow));
    }
}
