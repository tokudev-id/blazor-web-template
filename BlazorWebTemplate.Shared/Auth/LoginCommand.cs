using System.ComponentModel.DataAnnotations;

namespace BlazorWebTemplate.Shared.Auth;

public sealed class LoginCommand
{
    [Required]
    [StringLength(64, MinimumLength = 3)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(128, MinimumLength = 4)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
