using BlazorWebTemplate.Shared.Common.Constants;
using BlazorWebTemplate.Shared.Common.Responses;

namespace BlazorWebTemplate.Client.Services.BackEnd.Rbac;

internal sealed class RbacService(IRbacApi rbacApi) : IRbacService
{
    public Task<ApiResult<List<string>>> GetPermissionsAsync(CancellationToken cancellationToken = default)
        => rbacApi.GetPermissionsAsync(cancellationToken);

    public Task<ApiResult<Guid>> CreateRoleAsync(string name, IEnumerable<string> permissions, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Task.FromResult(ApiResult<Guid>.Failure(new ApiError(ApiErrorCodes.Validation, "Role name is required.")));

        return rbacApi.CreateRoleAsync(name, permissions, cancellationToken);
    }

    public Task<ApiResult<List<string>>> GetUserRolesAsync(string userId, CancellationToken cancellationToken = default)
        => rbacApi.GetUserRolesAsync(userId, cancellationToken);

    public Task<ApiResult> AssignRoleAsync(string userId, string roleName, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            return Task.FromResult(ApiResult.Failure(new ApiError(ApiErrorCodes.Validation, "Role name is required.")));

        return rbacApi.AssignRoleAsync(userId, roleName, cancellationToken);
    }
}
