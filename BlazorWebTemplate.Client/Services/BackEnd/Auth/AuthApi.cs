using System.Net.Http.Json;
using BlazorWebTemplate.Client.Services.BackEnd.Infrastructure.Http;
using BlazorWebTemplate.Shared.Auth;
using BlazorWebTemplate.Shared.Common;
using Microsoft.Extensions.Logging;

namespace BlazorWebTemplate.Client.Services.BackEnd.Auth;

internal sealed class AuthApi(
    IHttpClientFactory httpClientFactory,
    ILogger<AuthApi> logger) : BaseApiService(logger), IAuthApi
{
    public async Task<ApiResult<DummyJsonLoginResponseDto>> LoginAsync(LoginCommand command, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(DependencyInjection.AuthClientName);
        using var request = new HttpRequestMessage(HttpMethod.Post, "auth/login")
        {
            Content = JsonContent.Create(new DummyJsonLoginRequestDto
            {
                Username = command.Username,
                Password = command.Password,
                ExpiresInMins = 30,
            }),
        };

        return await SendForResultAsync<DummyJsonLoginResponseDto>(client, request, cancellationToken);
    }

    public async Task<ApiResult<DummyJsonRefreshResponseDto>> RefreshAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(DependencyInjection.AuthClientName);
        using var request = new HttpRequestMessage(HttpMethod.Post, "auth/refresh")
        {
            Content = JsonContent.Create(new DummyJsonRefreshRequestDto
            {
                RefreshToken = refreshToken,
                ExpiresInMins = 30,
            }),
        };

        return await SendForResultAsync<DummyJsonRefreshResponseDto>(client, request, cancellationToken);
    }

    public async Task<ApiResult<DummyJsonCurrentUserDto>> GetCurrentUserAsync(string accessToken, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(DependencyInjection.AuthClientName);
        using var request = new HttpRequestMessage(HttpMethod.Get, "auth/me");
        request.SetBearerToken(accessToken);

        return await SendForResultAsync<DummyJsonCurrentUserDto>(client, request, cancellationToken);
    }
}
