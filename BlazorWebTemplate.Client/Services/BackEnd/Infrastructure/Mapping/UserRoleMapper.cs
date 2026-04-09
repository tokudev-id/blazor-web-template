using BlazorWebTemplate.Shared.Services.Authorization.Constants;

namespace BlazorWebTemplate.Client.Services.BackEnd.Infrastructure.Mapping;

internal sealed class UserRoleMapper : IUserRoleMapper
{
    public string MapToAppRole(string? backendRole) => backendRole?.Trim().ToLowerInvariant() switch
    {
        "admin" => RoleNameFor.Admin,
        "moderator" => RoleNameFor.Editor,
        _ => RoleNameFor.Viewer,
    };
}
