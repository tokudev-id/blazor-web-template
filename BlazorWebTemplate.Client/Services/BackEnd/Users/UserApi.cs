using BlazorWebTemplate.Client.Services.BackEnd.Infrastructure.Http;
using BlazorWebTemplate.Shared.Common;
using Microsoft.Extensions.Logging;

namespace BlazorWebTemplate.Client.Services.BackEnd.Users;

internal sealed class UserApi(
    IHttpClientFactory httpClientFactory,
    ILogger<UserApi> logger) : BaseApiService(logger), IUserApi
{
    public async Task<ApiResult<DummyJsonUserListResponseDto>> GetUsersAsync(string? search, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(DependencyInjection.ApiClientName);
        var path = string.IsNullOrWhiteSpace(search)
            ? "users?limit=20&skip=0"
            : $"users/search?q={Uri.EscapeDataString(search)}";

        using var request = new HttpRequestMessage(HttpMethod.Get, path);
        return await SendForResultAsync<DummyJsonUserListResponseDto>(client, request, cancellationToken);
    }
}
