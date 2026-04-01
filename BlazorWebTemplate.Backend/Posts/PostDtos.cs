namespace BlazorWebTemplate.Backend.Posts;

internal sealed class DummyJsonPostListResponseDto
{
    public List<DummyJsonPostDto> Posts { get; set; } = [];
    public int Total { get; set; }
    public int Skip { get; set; }
    public int Limit { get; set; }
}

internal sealed class DummyJsonPostDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = [];
    public int Views { get; set; }
    public int UserId { get; set; }
    public DummyJsonReactionDto? Reactions { get; set; }
}

internal sealed class DummyJsonReactionDto
{
    public int Likes { get; set; }
    public int Dislikes { get; set; }
}

internal sealed class DummyJsonPostWriteDto
{
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = [];
    public int UserId { get; set; }
}
