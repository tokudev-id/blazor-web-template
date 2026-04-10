using System.Net.Http.Json;
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
    public async Task<ApiResult<UnictiveUserListResponseDto>> GetUsersAsync(string? search, int pageNumber, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(DependencyInjection.ApiClientName);
        var pageSize = backEndOptions.Value.DefaultPageSize;
        var path = string.IsNullOrWhiteSpace(search)
            ? $"api/v1/admin/userManagement?pageNumber={pageNumber}&pageSize={pageSize}"
            : $"api/v1/admin/userManagement?pageNumber={pageNumber}&pageSize={pageSize}&searchTerm={Uri.EscapeDataString(search)}";

        using var request = new HttpRequestMessage(HttpMethod.Get, path);
        return await SendForResultAsync<UnictiveUserListResponseDto>(client, request, cancellationToken);
    }

    public async Task<ApiResult<UnictiveUserDto>> GetUserAsync(string userId, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(DependencyInjection.ApiClientName);
        using var request = new HttpRequestMessage(HttpMethod.Get, $"api/v1/admin/userManagement/{Uri.EscapeDataString(userId)}");
        return await SendForResultAsync<UnictiveUserDto>(client, request, cancellationToken);
    }

    public async Task<ApiResult<UnictiveUserDto>> UpdateUserAsync(string userId, UnictiveUpdateUserRequestDto requestDto, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(DependencyInjection.ApiClientName);
        using var request = new HttpRequestMessage(HttpMethod.Put, $"api/v1/admin/userManagement/{Uri.EscapeDataString(userId)}")
        {
            Content = JsonContent.Create(requestDto),
        };
        return await SendForResultAsync<UnictiveUserDto>(client, request, cancellationToken);
    }

    public async Task<ApiResult> DeleteUserAsync(string userId, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(DependencyInjection.ApiClientName);
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"api/v1/admin/userManagement/{Uri.EscapeDataString(userId)}");
        return await SendAsync(client, request, cancellationToken);
    }

    public async Task<ApiResult> RegisterUserAsync(UnictiveRegisterUserRequestDto requestDto, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(DependencyInjection.ApiClientName);
        using var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/Auth/register")
        {
            Content = JsonContent.Create(requestDto),
        };
        return await SendAsync(client, request, cancellationToken);
    }

    public async Task<ApiResult> ActivateUserAsync(string userId, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(DependencyInjection.ApiClientName);
        using var request = new HttpRequestMessage(HttpMethod.Post, $"api/v1/admin/UserManagement/{Uri.EscapeDataString(userId)}/activate");
        return await SendAsync(client, request, cancellationToken);
    }

    public async Task<ApiResult> DeactivateUserAsync(string userId, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(DependencyInjection.ApiClientName);
        using var request = new HttpRequestMessage(HttpMethod.Post, $"api/v1/admin/UserManagement/{Uri.EscapeDataString(userId)}/deactivate");
        return await SendAsync(client, request, cancellationToken);
    }
}
