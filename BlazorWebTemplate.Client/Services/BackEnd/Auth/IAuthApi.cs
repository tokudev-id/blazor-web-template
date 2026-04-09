using BlazorWebTemplate.Shared.Common.Responses;
using BlazorWebTemplate.Shared.Services.Authentication.Commands.Login;

namespace BlazorWebTemplate.Client.Services.BackEnd.Auth;

internal interface IAuthApi
{
    Task<ApiResult<UnictiveLoginResponseDto>> LoginAsync(LoginCommand command, CancellationToken cancellationToken);

    Task<ApiResult<UnictiveLoginResponseDto>> RefreshAsync(string accessToken, string refreshToken, CancellationToken cancellationToken);

    Task<ApiResult<UnictiveCurrentUserDto>> GetCurrentUserAsync(string accessToken, CancellationToken cancellationToken);
}
