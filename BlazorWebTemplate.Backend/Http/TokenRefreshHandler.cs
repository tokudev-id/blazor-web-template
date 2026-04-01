using BlazorWebTemplate.Backend.Auth;
using BlazorWebTemplate.Backend.Session;
using BlazorWebTemplate.Shared.Common;

namespace BlazorWebTemplate.Backend.Http;

internal sealed class TokenRefreshHandler(
    ITokenStore tokenStore,
    DummyJsonAuthApiClient authApiClient) : DelegatingHandler
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

        var refreshToken = await tokenStore.GetRefreshTokenAsync(cancellationToken);
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            await tokenStore.ClearAsync(cancellationToken);
            return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized)
            {
                ReasonPhrase = "The session expired and no refresh token was available.",
                RequestMessage = clonedRequest,
            };
        }

        var refreshResult = await authApiClient.RefreshAsync(refreshToken, cancellationToken);
        if (refreshResult.IsFailure || refreshResult.Value is null)
        {
            await tokenStore.ClearAsync(cancellationToken);
            return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized)
            {
                ReasonPhrase = refreshResult.Error?.Message ?? "The session refresh failed.",
                RequestMessage = clonedRequest,
            };
        }

        var newExpiry = DateTimeOffset.UtcNow.AddMinutes(30);
        await tokenStore.RefreshAsync(refreshResult.Value.AccessToken, refreshResult.Value.RefreshToken, newExpiry, cancellationToken);

        clonedRequest.Options.Set(RetryKey, true);
        clonedRequest.SetBearerToken(refreshResult.Value.AccessToken);
        return await base.SendAsync(clonedRequest, cancellationToken);
    }
}
