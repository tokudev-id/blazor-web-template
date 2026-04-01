using BlazorWebTemplate.Backend.Session;
using BlazorWebTemplate.Shared.Auth;

namespace BlazorWebTemplate.Backend.Auth;

internal sealed class CurrentUserService(ITokenStore tokenStore) : ICurrentUserService
{
    public async Task<AppUser?> GetCurrentUserAsync(CancellationToken cancellationToken = default)
        => (await tokenStore.GetSessionAsync(cancellationToken))?.User;
}
