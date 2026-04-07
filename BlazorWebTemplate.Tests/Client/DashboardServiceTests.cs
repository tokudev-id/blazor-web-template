using BlazorWebTemplate.Client.Services.BackEnd.Auth;
using BlazorWebTemplate.Client.Services.BackEnd.Categories;
using BlazorWebTemplate.Client.Services.BackEnd.Dashboard;
using BlazorWebTemplate.Client.Services.BackEnd.Posts;
using BlazorWebTemplate.Client.Services.BackEnd.Users;
using BlazorWebTemplate.Shared.Auth;
using BlazorWebTemplate.Shared.Categories;
using BlazorWebTemplate.Shared.Common;
using BlazorWebTemplate.Shared.Dashboard;
using BlazorWebTemplate.Shared.Posts;
using BlazorWebTemplate.Shared.Users;

namespace BlazorWebTemplate.Tests.Client;

public sealed class DashboardServiceTests
{
    [Fact]
    public async Task GetSummaryAsync_ComposesCountsAndCurrentRole()
    {
        var service = new DashboardService(
            new FakePostService(),
            new FakeCategoryService(),
            new FakeUserService(),
            new FakeCurrentUserService());

        var result = await service.GetSummaryAsync();

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(7, result.Value!.TotalPosts);
        Assert.Equal(2, result.Value.TotalCategories);
        Assert.Equal(3, result.Value.TotalUsers);
        Assert.Equal(RoleNames.Admin, result.Value.MyRole);
        Assert.Equal(2, result.Value.RecentPosts.Count);
    }

    private sealed class FakePostService : IPostService
    {
        public Task<ApiResult<PagedResult<PostSummary>>> GetPostsAsync(PostQuery query, CancellationToken cancellationToken = default)
            => Task.FromResult(ApiResult<PagedResult<PostSummary>>.Success(new PagedResult<PostSummary>(
                [
                    new PostSummary(1, "Alpha", "Excerpt A", ["ops"], "User #1", 10, 2, 0),
                    new PostSummary(2, "Beta", "Excerpt B", ["news"], "User #2", 15, 4, 1)
                ],
                1,
                5,
                7)));

        public Task<ApiResult<PostDetail>> GetPostAsync(int id, CancellationToken cancellationToken = default)
            => throw new NotSupportedException();

        public Task<ApiResult<PostDetail>> CreateAsync(PostUpsertRequest request, CancellationToken cancellationToken = default)
            => throw new NotSupportedException();

        public Task<ApiResult<PostDetail>> UpdateAsync(int id, PostUpsertRequest request, CancellationToken cancellationToken = default)
            => throw new NotSupportedException();

        public Task<ApiResult> DeleteAsync(int id, CancellationToken cancellationToken = default)
            => throw new NotSupportedException();
    }

    private sealed class FakeCategoryService : ICategoryService
    {
        public Task<ApiResult<IReadOnlyList<CategorySummary>>> GetCategoriesAsync(string? search = null, CancellationToken cancellationToken = default)
            => Task.FromResult(ApiResult<IReadOnlyList<CategorySummary>>.Success(
                [
                    new CategorySummary("Ops", "ops", "Ops", "/posts?tag=ops"),
                    new CategorySummary("News", "news", "News", "/posts?tag=news")
                ]));
    }

    private sealed class FakeUserService : IUserService
    {
        public Task<ApiResult<IReadOnlyList<UserSummary>>> GetUsersAsync(string? search = null, CancellationToken cancellationToken = default)
            => Task.FromResult(ApiResult<IReadOnlyList<UserSummary>>.Success(
                [
                    new UserSummary(1, "A", "a@example.com", "a", RoleNames.Admin, "One", null),
                    new UserSummary(2, "B", "b@example.com", "b", RoleNames.Editor, "Two", null),
                    new UserSummary(3, "C", "c@example.com", "c", RoleNames.Viewer, "Three", null)
                ]));
    }

    private sealed class FakeCurrentUserService : ICurrentUserService
    {
        public Task<AppUser?> GetCurrentUserAsync(CancellationToken cancellationToken = default)
            => Task.FromResult<AppUser?>(new AppUser(1, "admin", "Admin", "admin@example.com", RoleNames.Admin, null));
    }
}
