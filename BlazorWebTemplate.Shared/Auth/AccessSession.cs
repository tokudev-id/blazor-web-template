namespace BlazorWebTemplate.Shared.Auth;

public sealed record AccessSession(
    string AccessToken,
    string RefreshToken,
    DateTimeOffset ExpiresAtUtc,
    AppUser User);
