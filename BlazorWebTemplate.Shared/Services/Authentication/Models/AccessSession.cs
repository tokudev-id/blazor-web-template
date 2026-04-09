namespace BlazorWebTemplate.Shared.Services.Authentication.Models;

public sealed record AccessSession(
    string AccessToken,
    string RefreshToken,
    DateTimeOffset ExpiresAtUtc,
    AppUser User);
