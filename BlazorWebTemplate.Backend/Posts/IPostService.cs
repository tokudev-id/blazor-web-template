using BlazorWebTemplate.Shared.Common;
using BlazorWebTemplate.Shared.Posts;

namespace BlazorWebTemplate.Backend.Posts;

public interface IPostService
{
    Task<ApiResult<PagedResult<PostSummary>>> GetPostsAsync(PostQuery query, CancellationToken cancellationToken = default);

    Task<ApiResult<PostDetail>> GetPostAsync(int id, CancellationToken cancellationToken = default);

    Task<ApiResult<PostDetail>> CreateAsync(PostUpsertRequest request, CancellationToken cancellationToken = default);

    Task<ApiResult<PostDetail>> UpdateAsync(int id, PostUpsertRequest request, CancellationToken cancellationToken = default);

    Task<ApiResult> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
