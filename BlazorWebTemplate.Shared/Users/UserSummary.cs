namespace BlazorWebTemplate.Shared.Users;

public sealed record UserSummary(
    int Id,
    string DisplayName,
    string Email,
    string Username,
    string Role,
    string CompanyName,
    string? AvatarUrl);
