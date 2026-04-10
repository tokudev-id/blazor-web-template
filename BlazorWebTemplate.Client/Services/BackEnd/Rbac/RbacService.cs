using BlazorWebTemplate.Shared.Common.Constants;
using BlazorWebTemplate.Shared.Common.Responses;
using BlazorWebTemplate.Shared.Services.Authorization.Models;

namespace BlazorWebTemplate.Client.Services.BackEnd.Rbac;

internal sealed class RbacService(IRbacApi rbacApi) : IRbacService
{
    public Task<ApiResult<List<string>>> GetPermissionsAsync(CancellationToken cancellationToken = default)
        => rbacApi.GetPermissionsAsync(cancellationToken);

    public async Task<ApiResult<List<RoleSummary>>> GetRolesAsync(CancellationToken cancellationToken = default)
    {
        var result = await rbacApi.GetRolesAsync(cancellationToken);
        if (result.IsFailure || result.Value is null)
            return ApiResult<List<RoleSummary>>.Failure(result.Error ?? new ApiError(ApiErrorCodes.Unknown, "Unable to load roles."));

        var roles = result.Value
            .Select(r => new RoleSummary(r.Id, r.Name, r.Permissions, r.UsersCount))
            .ToList();

        return ApiResult<List<RoleSummary>>.Success(roles);
    }

    public Task<ApiResult<Guid>> CreateRoleAsync(string name, IEnumerable<string> permissions, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Task.FromResult(ApiResult<Guid>.Failure(new ApiError(ApiErrorCodes.Validation, "Role name is required.")));

        return rbacApi.CreateRoleAsync(name, permissions, cancellationToken);
    }

    public Task<ApiResult> DeleteRoleAsync(string roleId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(roleId))
            return Task.FromResult(ApiResult.Failure(new ApiError(ApiErrorCodes.Validation, "Role ID is required.")));

        return rbacApi.DeleteRoleAsync(roleId, cancellationToken);
    }

    public async Task<ApiResult<RoleSummary>> GetRoleAsync(string roleId, CancellationToken cancellationToken = default)
    {
        var result = await rbacApi.GetRoleByIdAsync(roleId, cancellationToken);
        if (result.IsFailure || result.Value is null)
            return ApiResult<RoleSummary>.Failure(result.Error ?? new ApiError(ApiErrorCodes.Unknown, "Unable to load role."));

        var r = result.Value;
        return ApiResult<RoleSummary>.Success(new RoleSummary(r.Id, r.Name, r.Permissions, r.UsersCount));
    }

    public async Task<ApiResult<RoleSummary>> UpdateRoleAsync(string roleId, string name, CancellationToken cancellationToken = default)
    {
        var result = await rbacApi.UpdateRoleAsync(roleId, name, cancellationToken);
        if (result.IsFailure || result.Value is null)
            return ApiResult<RoleSummary>.Failure(result.Error ?? new ApiError(ApiErrorCodes.Unknown, "Unable to update role."));

        var r = result.Value;
        return ApiResult<RoleSummary>.Success(new RoleSummary(r.Id, r.Name, r.Permissions, r.UsersCount));
    }

    public async Task<ApiResult<RoleSummary>> UpdateRolePermissionsAsync(string roleId, IEnumerable<string> permissions, CancellationToken cancellationToken = default)
    {
        var result = await rbacApi.UpdateRolePermissionsAsync(roleId, permissions, cancellationToken);
        if (result.IsFailure || result.Value is null)
            return ApiResult<RoleSummary>.Failure(result.Error ?? new ApiError(ApiErrorCodes.Unknown, "Unable to update permissions."));

        var r = result.Value;
        return ApiResult<RoleSummary>.Success(new RoleSummary(r.Id, r.Name, r.Permissions, r.UsersCount));
    }

    public Task<ApiResult<List<string>>> GetUserRolesAsync(string userId, CancellationToken cancellationToken = default)
        => rbacApi.GetUserRolesAsync(userId, cancellationToken);

    public Task<ApiResult> AssignRoleAsync(string userId, string roleName, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            return Task.FromResult(ApiResult.Failure(new ApiError(ApiErrorCodes.Validation, "Role name is required.")));

        return rbacApi.AssignRoleAsync(userId, roleName, cancellationToken);
    }

    public Task<ApiResult> RemoveRoleFromUserAsync(string userId, string roleName, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            return Task.FromResult(ApiResult.Failure(new ApiError(ApiErrorCodes.Validation, "Role name is required.")));

        return rbacApi.RemoveRoleFromUserAsync(userId, roleName, cancellationToken);
    }
}
