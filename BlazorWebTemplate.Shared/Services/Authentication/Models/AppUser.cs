namespace BlazorWebTemplate.Shared.Services.Authentication.Models;

public sealed record AppUser(
    string Id,
    string Username,
    string DisplayName,
    string Email,
    string Role,
    string? AvatarUrl);
