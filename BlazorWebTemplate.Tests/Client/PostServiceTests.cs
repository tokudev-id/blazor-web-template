using BlazorWebTemplate.Client.Services.BackEnd.Auth;
using BlazorWebTemplate.Client.Services.BackEnd.Posts;
using BlazorWebTemplate.Shared.Auth;
using BlazorWebTemplate.Shared.Common;
using BlazorWebTemplate.Shared.Posts;

namespace BlazorWebTemplate.Tests.Client;

public sealed class PostServiceTests
{
    [Fact]
    public async Task GetPostsAsync_AppliesTagFilterAndSortsByQuery()
    {
        var api = new FakePostApi(ApiResult<DummyJsonPostListResponseDto>.Success(new DummyJsonPostListResponseDto
        {
            Total = 3,
            Posts =
            [
                new DummyJsonPostDto { Id = 2, Title = "Zeta", Body = "Body Z", Tags = ["ops"], UserId = 7, Views = 20 },
                new DummyJsonPostDto { Id = 1, Title = "Alpha", Body = "Body A", Tags = ["ops", "cms"], UserId = 5, Views = 10 },
                new DummyJsonPostDto { Id = 3, Title = "Beta", Body = "Body B", Tags = ["news"], UserId = 6, Views = 15 }
            ]
        }));

        var service = new PostService(api, new FakeCurrentUserService());

        var result = await service.GetPostsAsync(new PostQuery(PageNumber: 1, PageSize: 10, Tag: "ops", SortBy: "title", SortDirection: SortDirection.Ascending));

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(2, result.Value!.Items.Count);
        Assert.Equal(["Alpha", "Zeta"], result.Value.Items.Select(post => post.Title).ToArray());
    }

    private sealed class FakePostApi(ApiResult<DummyJsonPostListResponseDto> listResult) : IPostApi
    {
        public Task<ApiResult<DummyJsonPostListResponseDto>> GetPostsAsync(PostQuery query, CancellationToken cancellationToken)
            => Task.FromResult(listResult);

        public Task<ApiResult<DummyJsonPostDto>> GetPostAsync(int id, CancellationToken cancellationToken)
            => Task.FromResult(ApiResult<DummyJsonPostDto>.Failure(new ApiError(ApiErrorCodes.NotFound, "Not used.")));

        public Task<ApiResult<DummyJsonPostDto>> CreateAsync(DummyJsonPostWriteDto dto, CancellationToken cancellationToken)
            => Task.FromResult(ApiResult<DummyJsonPostDto>.Failure(new ApiError(ApiErrorCodes.NotFound, "Not used.")));

        public Task<ApiResult<DummyJsonPostDto>> UpdateAsync(int id, DummyJsonPostWriteDto dto, CancellationToken cancellationToken)
            => Task.FromResult(ApiResult<DummyJsonPostDto>.Failure(new ApiError(ApiErrorCodes.NotFound, "Not used.")));

        public Task<ApiResult> DeleteAsync(int id, CancellationToken cancellationToken)
            => Task.FromResult(ApiResult.Failure(new ApiError(ApiErrorCodes.NotFound, "Not used.")));
    }

    private sealed class FakeCurrentUserService : ICurrentUserService
    {
        public Task<AppUser?> GetCurrentUserAsync(CancellationToken cancellationToken = default)
            => Task.FromResult<AppUser?>(new AppUser(9, "demo", "Demo User", "demo@example.com", RoleNames.Editor, null));
    }
}
