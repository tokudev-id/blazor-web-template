using BlazorWebTemplate.Shared.Common.Responses;
using BlazorWebTemplate.Shared.Services.Authorization.Models;

namespace BlazorWebTemplate.Client.Services.BackEnd.Rbac;

public interface IRbacService
{
    Task<ApiResult<List<string>>> GetPermissionsAsync(CancellationToken cancellationToken = default);
    Task<ApiResult<List<RoleSummary>>> GetRolesAsync(CancellationToken cancellationToken = default);
    Task<ApiResult<Guid>> CreateRoleAsync(string name, IEnumerable<string> permissions, CancellationToken cancellationToken = default);
    Task<ApiResult> DeleteRoleAsync(string roleId, CancellationToken cancellationToken = default);
    Task<ApiResult<RoleSummary>> GetRoleAsync(string roleId, CancellationToken cancellationToken = default);
    Task<ApiResult<RoleSummary>> UpdateRoleAsync(string roleId, string name, CancellationToken cancellationToken = default);
    Task<ApiResult<RoleSummary>> UpdateRolePermissionsAsync(string roleId, IEnumerable<string> permissions, CancellationToken cancellationToken = default);
    Task<ApiResult<List<string>>> GetUserRolesAsync(string userId, CancellationToken cancellationToken = default);
    Task<ApiResult> AssignRoleAsync(string userId, string roleName, CancellationToken cancellationToken = default);
    Task<ApiResult> RemoveRoleFromUserAsync(string userId, string roleName, CancellationToken cancellationToken = default);
}
