namespace BlazorWebTemplate.Shared.Posts;

public sealed record PostDetail(
    int Id,
    string Title,
    string Body,
    IReadOnlyList<string> Tags,
    string AuthorName,
    int Views,
    int Likes,
    int Dislikes,
    int UserId);
