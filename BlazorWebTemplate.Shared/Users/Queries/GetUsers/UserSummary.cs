namespace BlazorWebTemplate.Shared.Users.Queries.GetUsers;

public sealed record UserSummary(
    string Id,
    string DisplayName,
    string Email,
    IReadOnlyList<string> Roles,
    bool IsActive,
    string? FirstName = null,
    string? LastName = null);
