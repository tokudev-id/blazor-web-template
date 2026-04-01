namespace BlazorWebTemplate.Shared.Auth;

public sealed record AppUser(
    int Id,
    string Username,
    string DisplayName,
    string Email,
    string Role,
    string? AvatarUrl);
