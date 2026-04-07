using BlazorWebTemplate.Client.Services.BackEnd.Infrastructure.Session;
using BlazorWebTemplate.Shared.Auth;

namespace BlazorWebTemplate.Client.Services.BackEnd.Auth;

internal sealed class CurrentUserService(ITokenStore tokenStore) : ICurrentUserService
{
    public async Task<AppUser?> GetCurrentUserAsync(CancellationToken cancellationToken = default)
        => (await tokenStore.GetSessionAsync(cancellationToken))?.User;
}
