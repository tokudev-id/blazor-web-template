using System.Net.Http.Json;
using BlazorWebTemplate.Backend.Extensions;
using BlazorWebTemplate.Backend.Http;
using BlazorWebTemplate.Shared.Auth;
using BlazorWebTemplate.Shared.Common;

namespace BlazorWebTemplate.Backend.Auth;

internal sealed class DummyJsonAuthApiClient(IHttpClientFactory httpClientFactory) : ApiClientBase
{
    public async Task<ApiResult<DummyJsonLoginResponseDto>> LoginAsync(LoginCommand command, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(ServiceRegistration.AuthClientName);
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
        var client = httpClientFactory.CreateClient(ServiceRegistration.AuthClientName);
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
        var client = httpClientFactory.CreateClient(ServiceRegistration.AuthClientName);
        using var request = new HttpRequestMessage(HttpMethod.Get, "auth/me");
        request.SetBearerToken(accessToken);

        return await SendForResultAsync<DummyJsonCurrentUserDto>(client, request, cancellationToken);
    }
}
