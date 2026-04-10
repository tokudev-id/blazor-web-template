using System.Net.Http.Json;
using BlazorWebTemplate.Client.Services.BackEnd.Infrastructure.Http;
using BlazorWebTemplate.Shared.Common.Responses;
using Microsoft.Extensions.Logging;

namespace BlazorWebTemplate.Client.Services.BackEnd.Rbac;

internal sealed class RbacApi(
    IHttpClientFactory httpClientFactory,
    ILogger<RbacApi> logger) : BaseApiService(logger), IRbacApi
{
    public async Task<ApiResult<List<string>>> GetPermissionsAsync(CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(DependencyInjection.ApiClientName);
        using var request = new HttpRequestMessage(HttpMethod.Get, "api/v1/rbac/permissions");
        return await SendForResultAsync<List<string>>(client, request, cancellationToken);
    }

    public async Task<ApiResult<List<UnictiveRoleDto>>> GetRolesAsync(CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(DependencyInjection.ApiClientName);
        using var request = new HttpRequestMessage(HttpMethod.Get, "api/v1/rbac/roles");
        return await SendForResultAsync<List<UnictiveRoleDto>>(client, request, cancellationToken);
    }

    public async Task<ApiResult> DeleteRoleAsync(string roleId, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(DependencyInjection.ApiClientName);
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"api/v1/rbac/roles/{Uri.EscapeDataString(roleId)}");
        return await SendAsync(client, request, cancellationToken);
    }

    public async Task<ApiResult<Guid>> CreateRoleAsync(string name, IEnumerable<string> permissions, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(DependencyInjection.ApiClientName);
        using var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/rbac/roles")
        {
            Content = JsonContent.Create(new UnictiveCreateRoleRequestDto
            {
                Name = name,
                Permissions = permissions.ToList(),
            }),
        };
        return await SendForResultAsync<Guid>(client, request, cancellationToken);
    }

    public async Task<ApiResult<UnictiveRoleDto>> GetRoleByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(DependencyInjection.ApiClientName);
        using var request = new HttpRequestMessage(HttpMethod.Get, $"api/v1/rbac/roles/{Uri.EscapeDataString(roleId)}");
        return await SendForResultAsync<UnictiveRoleDto>(client, request, cancellationToken);
    }

    public async Task<ApiResult<UnictiveRoleDto>> UpdateRoleAsync(string roleId, string name, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(DependencyInjection.ApiClientName);
        using var request = new HttpRequestMessage(HttpMethod.Put, $"api/v1/rbac/roles/{Uri.EscapeDataString(roleId)}")
        {
            Content = JsonContent.Create(new UnictiveUpdateRoleRequestDto { Name = name }),
        };
        return await SendForResultAsync<UnictiveRoleDto>(client, request, cancellationToken);
    }

    public async Task<ApiResult<UnictiveRoleDto>> UpdateRolePermissionsAsync(string roleId, IEnumerable<string> permissions, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(DependencyInjection.ApiClientName);
        using var request = new HttpRequestMessage(HttpMethod.Put, $"api/v1/rbac/roles/{Uri.EscapeDataString(roleId)}/permissions")
        {
            Content = JsonContent.Create(new UnictiveUpdateRolePermissionsRequestDto { Permissions = permissions.ToList() }),
        };
        return await SendForResultAsync<UnictiveRoleDto>(client, request, cancellationToken);
    }

    public async Task<ApiResult<List<string>>> GetUserRolesAsync(string userId, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(DependencyInjection.ApiClientName);
        using var request = new HttpRequestMessage(HttpMethod.Get, $"api/v1/rbac/users/{Uri.EscapeDataString(userId)}/roles");
        return await SendForResultAsync<List<string>>(client, request, cancellationToken);
    }

    public async Task<ApiResult> AssignRoleAsync(string userId, string roleName, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(DependencyInjection.ApiClientName);
        using var request = new HttpRequestMessage(HttpMethod.Post, $"api/v1/rbac/users/{Uri.EscapeDataString(userId)}/roles")
        {
            Content = JsonContent.Create(roleName),
        };
        return await SendAsync(client, request, cancellationToken);
    }

    public async Task<ApiResult> RemoveRoleFromUserAsync(string userId, string roleName, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(DependencyInjection.ApiClientName);
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"api/v1/rbac/users/{Uri.EscapeDataString(userId)}/roles/{Uri.EscapeDataString(roleName)}");
        return await SendAsync(client, request, cancellationToken);
    }
}
