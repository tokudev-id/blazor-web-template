using BlazorWebTemplate.Client.Services.BackEnd.Infrastructure.Http;
using BlazorWebTemplate.Shared.Common.Responses;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BlazorWebTemplate.Client.Services.BackEnd.Users;

internal sealed class UserApi(
    IHttpClientFactory httpClientFactory,
    IOptions<BackEndOptions> backEndOptions,
    ILogger<UserApi> logger) : BaseApiService(logger), IUserApi
{
    public async Task<ApiResult<UnictiveUserListResponseDto>> GetUsersAsync(string? search, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(DependencyInjection.ApiClientName);
        var pageSize = backEndOptions.Value.DefaultPageSize;
        var path = string.IsNullOrWhiteSpace(search)
            ? $"api/v1/admin/user-management?pageNumber=1&pageSize={pageSize}"
            : $"api/v1/admin/user-management?pageNumber=1&pageSize={pageSize}&searchTerm={Uri.EscapeDataString(search)}";

        using var request = new HttpRequestMessage(HttpMethod.Get, path);
        return await SendForResultAsync<UnictiveUserListResponseDto>(client, request, cancellationToken);
    }
}
