using BlazorWebTemplate.Client.Services.BackEnd.Infrastructure.Session;

namespace BlazorWebTemplate.Client.Services.BackEnd.Infrastructure.Http;

internal sealed class AccessTokenHandler(ITokenStore tokenStore) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var accessToken = await tokenStore.GetAccessTokenAsync(cancellationToken);
        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            request.SetBearerToken(accessToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
