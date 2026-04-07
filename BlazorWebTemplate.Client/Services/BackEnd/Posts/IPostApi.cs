using BlazorWebTemplate.Shared.Common;
using BlazorWebTemplate.Shared.Posts;

namespace BlazorWebTemplate.Client.Services.BackEnd.Posts;

internal interface IPostApi
{
    Task<ApiResult<DummyJsonPostListResponseDto>> GetPostsAsync(PostQuery query, CancellationToken cancellationToken);

    Task<ApiResult<DummyJsonPostDto>> GetPostAsync(int id, CancellationToken cancellationToken);

    Task<ApiResult<DummyJsonPostDto>> CreateAsync(DummyJsonPostWriteDto dto, CancellationToken cancellationToken);

    Task<ApiResult<DummyJsonPostDto>> UpdateAsync(int id, DummyJsonPostWriteDto dto, CancellationToken cancellationToken);

    Task<ApiResult> DeleteAsync(int id, CancellationToken cancellationToken);
}
