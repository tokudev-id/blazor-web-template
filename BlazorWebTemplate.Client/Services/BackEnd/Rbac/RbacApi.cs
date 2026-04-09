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
}
