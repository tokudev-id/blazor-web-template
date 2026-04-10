namespace BlazorWebTemplate.Shared.Users.Queries.GetUsers;

public sealed record CreateUserRequest(
    string Email,
    string Password,
    string ConfirmPassword,
    string? FirstName,
    string? LastName);
