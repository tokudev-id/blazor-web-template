namespace BlazorWebTemplate.Shared.Posts;

public sealed record PostSummary(
    int Id,
    string Title,
    string Excerpt,
    IReadOnlyList<string> Tags,
    string AuthorName,
    int Views,
    int Likes,
    int Dislikes);
