using System.Net.Http.Json;
using BlazorWebTemplate.Client.Services.BackEnd.Infrastructure.Http;
using BlazorWebTemplate.Shared.Common.Responses;
using BlazorWebTemplate.Shared.Services.Authentication.Commands.Login;
using Microsoft.Extensions.Logging;

namespace BlazorWebTemplate.Client.Services.BackEnd.Auth;

internal sealed class AuthApi(
    IHttpClientFactory httpClientFactory,
    ILogger<AuthApi> logger) : BaseApiService(logger), IAuthApi
{
    public async Task<ApiResult<UnictiveLoginResponseDto>> LoginAsync(LoginCommand command, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(DependencyInjection.AuthClientName);
        using var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/auth/login")
        {
            Content = JsonContent.Create(new UnictiveLoginRequestDto
            {
                Email = command.Email,
                Password = command.Password,
            }),
        };

        return await SendForResultAsync<UnictiveLoginResponseDto>(client, request, cancellationToken);
    }

    public async Task<ApiResult<UnictiveLoginResponseDto>> RefreshAsync(string accessToken, string refreshToken, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(DependencyInjection.AuthClientName);
        using var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/auth/refresh-token")
        {
            Content = JsonContent.Create(new UnictiveRefreshRequestDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            }),
        };

        return await SendForResultAsync<UnictiveLoginResponseDto>(client, request, cancellationToken);
    }

    public async Task<ApiResult<UnictiveCurrentUserDto>> GetCurrentUserAsync(string accessToken, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(DependencyInjection.AuthClientName);
        using var request = new HttpRequestMessage(HttpMethod.Get, "api/v1/users/me");
        request.SetBearerToken(accessToken);

        return await SendForResultAsync<UnictiveCurrentUserDto>(client, request, cancellationToken);
    }

    public async Task<ApiResult<UnictiveCurrentUserDto>> UpdateProfileAsync(UnictiveUpdateProfileRequestDto requestDto, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(DependencyInjection.ApiClientName);
        using var request = new HttpRequestMessage(HttpMethod.Put, "api/v1/users/me")
        {
            Content = JsonContent.Create(requestDto),
        };

        return await SendForResultAsync<UnictiveCurrentUserDto>(client, request, cancellationToken);
    }

    public async Task<ApiResult> ChangePasswordAsync(UnictiveChangePasswordRequestDto requestDto, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(DependencyInjection.ApiClientName);
        using var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/auth/change-password")
        {
            Content = JsonContent.Create(requestDto),
        };

        return await SendAsync(client, request, cancellationToken);
    }
}
