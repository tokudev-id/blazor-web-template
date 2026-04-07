using BlazorWebTemplate.Shared.Auth;

namespace BlazorWebTemplate.Client.Services.BackEnd.Infrastructure.Mapping;

internal sealed class UserRoleMapper : IUserRoleMapper
{
    public string MapToAppRole(string? backendRole) => backendRole?.Trim().ToLowerInvariant() switch
    {
        "admin" => RoleNames.Admin,
        "moderator" => RoleNames.Editor,
        _ => RoleNames.Viewer,
    };
}
