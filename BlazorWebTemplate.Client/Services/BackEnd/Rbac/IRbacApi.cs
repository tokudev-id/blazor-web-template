using BlazorWebTemplate.Shared.Common.Responses;

namespace BlazorWebTemplate.Client.Services.BackEnd.Rbac;

internal interface IRbacApi
{
    Task<ApiResult<List<string>>> GetPermissionsAsync(CancellationToken cancellationToken);

    Task<ApiResult<Guid>> CreateRoleAsync(string name, IEnumerable<string> permissions, CancellationToken cancellationToken);

    Task<ApiResult<List<string>>> GetUserRolesAsync(string userId, CancellationToken cancellationToken);

    Task<ApiResult> AssignRoleAsync(string userId, string roleName, CancellationToken cancellationToken);
}
