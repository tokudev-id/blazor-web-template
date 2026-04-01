using System.ComponentModel.DataAnnotations;

namespace BlazorWebTemplate.Shared.Posts;

public sealed class PostUpsertRequest
{
    [Required]
    [StringLength(120, MinimumLength = 6)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(4000, MinimumLength = 30)]
    public string Body { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public string Tags { get; set; } = string.Empty;
}
