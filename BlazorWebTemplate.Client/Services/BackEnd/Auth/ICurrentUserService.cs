using BlazorWebTemplate.Shared.Auth;

namespace BlazorWebTemplate.Client.Services.BackEnd.Auth;

public interface ICurrentUserService
{
    Task<AppUser?> GetCurrentUserAsync(CancellationToken cancellationToken = default);
}
