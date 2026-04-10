using BlazorWebTemplate.Shared.Common.Responses;

namespace BlazorWebTemplate.Client.Services.BackEnd.Rbac;

internal interface IRbacApi
{
    Task<ApiResult<List<string>>> GetPermissionsAsync(CancellationToken cancellationToken);
    Task<ApiResult<List<UnictiveRoleDto>>> GetRolesAsync(CancellationToken cancellationToken);
    Task<ApiResult<Guid>> CreateRoleAsync(string name, IEnumerable<string> permissions, CancellationToken cancellationToken);
    Task<ApiResult> DeleteRoleAsync(string roleId, CancellationToken cancellationToken);
    Task<ApiResult<UnictiveRoleDto>> GetRoleByIdAsync(string roleId, CancellationToken cancellationToken);
    Task<ApiResult<UnictiveRoleDto>> UpdateRoleAsync(string roleId, string name, CancellationToken cancellationToken);
    Task<ApiResult<UnictiveRoleDto>> UpdateRolePermissionsAsync(string roleId, IEnumerable<string> permissions, CancellationToken cancellationToken);
    Task<ApiResult<List<string>>> GetUserRolesAsync(string userId, CancellationToken cancellationToken);
    Task<ApiResult> AssignRoleAsync(string userId, string roleName, CancellationToken cancellationToken);
    Task<ApiResult> RemoveRoleFromUserAsync(string userId, string roleName, CancellationToken cancellationToken);
}
