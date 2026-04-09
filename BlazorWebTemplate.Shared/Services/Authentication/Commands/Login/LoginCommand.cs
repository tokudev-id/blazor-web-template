using System.ComponentModel.DataAnnotations;

namespace BlazorWebTemplate.Shared.Services.Authentication.Commands.Login;

public sealed class LoginCommand
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(128, MinimumLength = 4)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
