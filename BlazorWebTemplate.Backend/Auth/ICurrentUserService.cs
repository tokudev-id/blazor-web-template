using BlazorWebTemplate.Shared.Auth;

namespace BlazorWebTemplate.Backend.Auth;

public interface ICurrentUserService
{
    Task<AppUser?> GetCurrentUserAsync(CancellationToken cancellationToken = default);
}
