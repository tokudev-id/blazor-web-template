using BlazorWebTemplate.Client.Services.BackEnd.Auth;
using BlazorWebTemplate.Shared.Common;
using BlazorWebTemplate.Shared.Posts;

namespace BlazorWebTemplate.Client.Services.BackEnd.Posts;

internal sealed class PostService(
    IPostApi postsApiClient,
    ICurrentUserService currentUserService) : IPostService
{
    public async Task<ApiResult<PagedResult<PostSummary>>> GetPostsAsync(PostQuery query, CancellationToken cancellationToken = default)
    {
        var result = await postsApiClient.GetPostsAsync(query, cancellationToken);
        if (result.IsFailure || result.Value is null)
        {
            return ApiResult<PagedResult<PostSummary>>.Failure(result.Error ?? new ApiError(ApiErrorCodes.Unknown, "Unable to load posts."));
        }

        var items = result.Value.Posts
            .Select(MapSummary)
            .Where(post => string.IsNullOrWhiteSpace(query.Tag) || post.Tags.Any(tag => tag.Equals(query.Tag, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        items = Sort(items, query).ToList();

        var totalCount = string.IsNullOrWhiteSpace(query.Tag) ? result.Value.Total : items.Count;
        return ApiResult<PagedResult<PostSummary>>.Success(new PagedResult<PostSummary>(items, query.NormalizedPageNumber, query.NormalizedPageSize, totalCount));
    }

    public async Task<ApiResult<PostDetail>> GetPostAsync(int id, CancellationToken cancellationToken = default)
    {
        var result = await postsApiClient.GetPostAsync(id, cancellationToken);
        return result.IsFailure || result.Value is null
            ? ApiResult<PostDetail>.Failure(result.Error ?? new ApiError(ApiErrorCodes.Unknown, "Unable to load the post."))
            : ApiResult<PostDetail>.Success(MapDetail(result.Value));
    }

    public async Task<ApiResult<PostDetail>> CreateAsync(PostUpsertRequest request, CancellationToken cancellationToken = default)
    {
        var currentUser = await currentUserService.GetCurrentUserAsync(cancellationToken);
        var dto = ToWriteDto(request, currentUser?.Id ?? 1);
        var result = await postsApiClient.CreateAsync(dto, cancellationToken);
        return result.IsFailure || result.Value is null
            ? ApiResult<PostDetail>.Failure(result.Error ?? new ApiError(ApiErrorCodes.Unknown, "Unable to create the post."))
            : ApiResult<PostDetail>.Success(MapDetail(result.Value));
    }

    public async Task<ApiResult<PostDetail>> UpdateAsync(int id, PostUpsertRequest request, CancellationToken cancellationToken = default)
    {
        var currentUser = await currentUserService.GetCurrentUserAsync(cancellationToken);
        var dto = ToWriteDto(request, currentUser?.Id ?? 1);
        var result = await postsApiClient.UpdateAsync(id, dto, cancellationToken);
        return result.IsFailure || result.Value is null
            ? ApiResult<PostDetail>.Failure(result.Error ?? new ApiError(ApiErrorCodes.Unknown, "Unable to update the post."))
            : ApiResult<PostDetail>.Success(MapDetail(result.Value));
    }

    public async Task<ApiResult> DeleteAsync(int id, CancellationToken cancellationToken = default)
        => await postsApiClient.DeleteAsync(id, cancellationToken);

    private static DummyJsonPostWriteDto ToWriteDto(PostUpsertRequest request, int userId)
    {
        var tags = request.Tags
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        return new DummyJsonPostWriteDto
        {
            Title = request.Title.Trim(),
            Body = request.Body.Trim(),
            Tags = tags,
            UserId = userId,
        };
    }

    private static PostSummary MapSummary(DummyJsonPostDto dto)
        => new(
            dto.Id,
            dto.Title,
            dto.Body.Length <= 160 ? dto.Body : $"{dto.Body[..157]}...",
            dto.Tags,
            $"User #{dto.UserId}",
            dto.Views,
            dto.Reactions?.Likes ?? 0,
            dto.Reactions?.Dislikes ?? 0);

    private static PostDetail MapDetail(DummyJsonPostDto dto)
        => new(
            dto.Id,
            dto.Title,
            dto.Body,
            dto.Tags,
            $"User #{dto.UserId}",
            dto.Views,
            dto.Reactions?.Likes ?? 0,
            dto.Reactions?.Dislikes ?? 0,
            dto.UserId);

    private static IEnumerable<PostSummary> Sort(IEnumerable<PostSummary> posts, PostQuery query)
    {
        var ordered = query.SortBy.ToLowerInvariant() switch
        {
            "title" => posts.OrderBy(post => post.Title, StringComparer.OrdinalIgnoreCase),
            "author" => posts.OrderBy(post => post.AuthorName, StringComparer.OrdinalIgnoreCase),
            "views" => posts.OrderBy(post => post.Views),
            _ => posts.OrderBy(post => post.Id),
        };

        return query.SortDirection == SortDirection.Descending
            ? ordered.Reverse()
            : ordered;
    }
}
