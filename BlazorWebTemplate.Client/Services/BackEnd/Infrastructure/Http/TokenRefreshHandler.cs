using BlazorWebTemplate.Client.Services.BackEnd.Auth;
using BlazorWebTemplate.Client.Services.BackEnd.Infrastructure.Session;
using BlazorWebTemplate.Shared.Common.Responses;

namespace BlazorWebTemplate.Client.Services.BackEnd.Infrastructure.Http;

internal sealed class TokenRefreshHandler(
    ITokenStore tokenStore,
    IAuthApi authApiClient) : DelegatingHandler
{
    private static readonly HttpRequestOptionsKey<bool> RetryKey = new("token-refresh-retry");

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var clonedRequest = await request.CloneAsync(cancellationToken);
        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode != System.Net.HttpStatusCode.Unauthorized || request.Options.TryGetValue(RetryKey, out var alreadyRetried) && alreadyRetried)
        {
            return response;
        }

        response.Dispose();

        var accessToken = await tokenStore.GetAccessTokenAsync(cancellationToken);
        var refreshToken = await tokenStore.GetRefreshTokenAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(refreshToken) || string.IsNullOrWhiteSpace(accessToken))
        {
            await tokenStore.ClearAsync(cancellationToken);
            return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized)
            {
                ReasonPhrase = "The session expired and no refresh token was available.",
                RequestMessage = clonedRequest,
            };
        }

        var refreshResult = await authApiClient.RefreshAsync(accessToken, refreshToken, cancellationToken);
        if (refreshResult.IsFailure || refreshResult.Value is null)
        {
            await tokenStore.ClearAsync(cancellationToken);
            return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized)
            {
                ReasonPhrase = refreshResult.Error?.Message ?? "The session refresh failed.",
                RequestMessage = clonedRequest,
            };
        }

        var newExpiry = new DateTimeOffset(refreshResult.Value.ExpiresAt, TimeSpan.Zero);
        await tokenStore.RefreshAsync(refreshResult.Value.AccessToken, refreshResult.Value.RefreshToken, newExpiry, cancellationToken);

        clonedRequest.Options.Set(RetryKey, true);
        clonedRequest.SetBearerToken(refreshResult.Value.AccessToken);
        return await base.SendAsync(clonedRequest, cancellationToken);
    }
}
