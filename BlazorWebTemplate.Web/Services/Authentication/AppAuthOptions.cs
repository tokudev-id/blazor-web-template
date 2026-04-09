using System.ComponentModel.DataAnnotations;

namespace BlazorWebTemplate.Web.Services.Authentication;

public sealed class AppAuthOptions
{
    public const string SectionName = "AppAuth";

    [Required]
    [StringLength(64, MinimumLength = 3)]
    public string CookieName { get; set; } = "BlazorWebTemplate.Auth";

    [Range(1, 72)]
    public int ExpireTimeSpanHours { get; set; } = 8;
}
