using BlazorWebTemplate.Shared.Posts;
using BlazorWebTemplate.Web.Common.Constants;
using PostSortDirection = BlazorWebTemplate.Shared.Common.SortDirection;

namespace BlazorWebTemplate.Web.Features.Posts.State;

public sealed class PostsPageState
{
    private const string DefaultSortBy = "updated";
    private const int DefaultPageSize = 12;

    public string QueryInput { get; set; } = string.Empty;
    public string TagInput { get; set; } = string.Empty;
    public string SortByInput { get; set; } = DefaultSortBy;
    public PostSortDirection SortDirectionInput { get; set; } = PostSortDirection.Descending;
    public int PageSizeInput { get; set; } = DefaultPageSize;

    public PostQuery ApplyRouteParameters(int? page, string? query, string? tag, string? sort, string? direction, int? size)
    {
        QueryInput = query ?? string.Empty;
        TagInput = tag ?? string.Empty;
        SortByInput = string.IsNullOrWhiteSpace(sort) ? DefaultSortBy : sort;
        SortDirectionInput = Enum.TryParse<PostSortDirection>(direction, true, out var parsedDirection)
            ? parsedDirection
            : PostSortDirection.Descending;
        PageSizeInput = Math.Clamp(size ?? DefaultPageSize, 6, 24);

        return new PostQuery(
            PageNumber: Math.Max(page ?? 1, 1),
            PageSize: PageSizeInput,
            Search: query,
            Tag: tag,
            SortBy: SortByInput,
            SortDirection: SortDirectionInput);
    }

    public string BuildApplyLink()
        => BuildFilterLink(1, QueryInput, TagInput, SortByInput, SortDirectionInput, PageSizeInput);

    public string BuildPageLink(int pageNumber)
        => BuildFilterLink(pageNumber, QueryInput, TagInput, SortByInput, SortDirectionInput, PageSizeInput);

    public string BuildTagLink(string tag)
        => AppRoutes.PostsByTag(tag);

    private static string BuildFilterLink(int pageNumber, string? query, string? tag, string sortBy, PostSortDirection sortDirection, int pageSize)
    {
        var querySegments = new List<string> { $"page={pageNumber}" };

        if (!string.IsNullOrWhiteSpace(query))
        {
            querySegments.Add($"q={Uri.EscapeDataString(query)}");
        }

        if (!string.IsNullOrWhiteSpace(tag))
        {
            querySegments.Add($"tag={Uri.EscapeDataString(tag)}");
        }

        querySegments.Add($"sort={Uri.EscapeDataString(sortBy)}");
        querySegments.Add($"direction={sortDirection}");
        querySegments.Add($"size={pageSize}");

        return $"{AppRoutes.Posts}?{string.Join("&", querySegments)}";
    }
}
