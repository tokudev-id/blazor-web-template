using BlazorWebTemplate.Shared.Common.Responses;

namespace BlazorWebTemplate.Client.Services.BackEnd.Rbac;

public interface IRbacService
{
    Task<ApiResult<List<string>>> GetPermissionsAsync(CancellationToken cancellationToken = default);

    Task<ApiResult<Guid>> CreateRoleAsync(string name, IEnumerable<string> permissions, CancellationToken cancellationToken = default);

    Task<ApiResult<List<string>>> GetUserRolesAsync(string userId, CancellationToken cancellationToken = default);

    Task<ApiResult> AssignRoleAsync(string userId, string roleName, CancellationToken cancellationToken = default);
}
