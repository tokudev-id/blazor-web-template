namespace BlazorWebTemplate.Shared.Services.Authorization.Models;

public sealed record RoleSummary(string Id, string Name, IReadOnlyList<string> Permissions, int UsersCount = 0);
