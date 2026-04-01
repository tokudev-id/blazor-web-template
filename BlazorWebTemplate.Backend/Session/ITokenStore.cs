using BlazorWebTemplate.Shared.Auth;

namespace BlazorWebTemplate.Backend.Session;

public interface ITokenStore
{
    Task<AccessSession?> GetSessionAsync(CancellationToken cancellationToken = default);

    Task<string?> GetAccessTokenAsync(CancellationToken cancellationToken = default);

    Task<string?> GetRefreshTokenAsync(CancellationToken cancellationToken = default);

    Task StoreAsync(AccessSession session, CancellationToken cancellationToken = default);

    Task RefreshAsync(string accessToken, string refreshToken, DateTimeOffset expiresAtUtc, CancellationToken cancellationToken = default);

    Task ClearAsync(CancellationToken cancellationToken = default);
}
