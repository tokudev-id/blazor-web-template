using BlazorWebTemplate.Shared.Auth;
using BlazorWebTemplate.Shared.Common;

namespace BlazorWebTemplate.Client.Services.BackEnd.Auth;

internal interface IAuthApi
{
    Task<ApiResult<DummyJsonLoginResponseDto>> LoginAsync(LoginCommand command, CancellationToken cancellationToken);

    Task<ApiResult<DummyJsonRefreshResponseDto>> RefreshAsync(string refreshToken, CancellationToken cancellationToken);

    Task<ApiResult<DummyJsonCurrentUserDto>> GetCurrentUserAsync(string accessToken, CancellationToken cancellationToken);
}
