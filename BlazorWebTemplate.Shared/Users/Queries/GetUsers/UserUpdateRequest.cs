namespace BlazorWebTemplate.Shared.Users.Queries.GetUsers;

public sealed record UserUpdateRequest(string? FirstName, string? LastName, string? PhoneNumber);
