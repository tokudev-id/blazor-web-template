namespace BlazorWebTemplate.Shared.Users.Queries.GetUsers;

public sealed record UserSummary(
    string Id,
    string DisplayName,
    string Email,
    string Role,
    bool IsActive,
    string? FirstName = null,
    string? LastName = null);
