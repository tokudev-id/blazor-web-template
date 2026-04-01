using BlazorWebTemplate.Backend.Extensions;
using BlazorWebTemplate.Backend.Http;
using BlazorWebTemplate.Shared.Common;

namespace BlazorWebTemplate.Backend.Users;

internal sealed class DummyJsonUsersApiClient(IHttpClientFactory httpClientFactory) : ApiClientBase
{
    public async Task<ApiResult<DummyJsonUserListResponseDto>> GetUsersAsync(string? search, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(ServiceRegistration.ApiClientName);
        var path = string.IsNullOrWhiteSpace(search)
            ? "users?limit=20&skip=0"
            : $"users/search?q={Uri.EscapeDataString(search)}";

        using var request = new HttpRequestMessage(HttpMethod.Get, path);
        return await SendForResultAsync<DummyJsonUserListResponseDto>(client, request, cancellationToken);
    }
}
