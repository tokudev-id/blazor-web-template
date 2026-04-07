using BlazorWebTemplate.Shared.Common;

namespace BlazorWebTemplate.Shared.Posts;

public sealed record PostQuery(
    int PageNumber = 1,
    int PageSize = 12,
    string? Search = null,
    string? Tag = null,
    string SortBy = "updated",
    SortDirection SortDirection = SortDirection.Descending)
{
    public int NormalizedPageNumber => Math.Max(PageNumber, 1);

    public int NormalizedPageSize => Math.Clamp(PageSize, 6, 48);

    public int Skip => Math.Max(NormalizedPageNumber - 1, 0) * NormalizedPageSize;
}
