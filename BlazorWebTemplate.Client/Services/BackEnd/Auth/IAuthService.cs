using BlazorWebTemplate.Shared.Auth;
using BlazorWebTemplate.Shared.Common;

namespace BlazorWebTemplate.Client.Services.BackEnd.Auth;

public interface IAuthService
{
    Task<ApiResult<AppUser>> LoginAsync(LoginCommand command, CancellationToken cancellationToken = default);

    Task<ApiResult<AppUser>> GetCurrentUserAsync(CancellationToken cancellationToken = default);

    Task LogoutAsync(CancellationToken cancellationToken = default);
}
