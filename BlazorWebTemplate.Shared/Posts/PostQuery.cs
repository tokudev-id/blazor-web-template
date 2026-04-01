namespace BlazorWebTemplate.Shared.Posts;

public sealed record PostQuery(
    int PageNumber = 1,
    int PageSize = 12,
    string? Search = null,
    string? Tag = null)
{
    public int Skip => Math.Max(PageNumber - 1, 0) * PageSize;
}
