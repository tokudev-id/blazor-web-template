namespace BlazorWebTemplate.Backend.Mapping;

public interface IUserRoleMapper
{
    string MapToAppRole(string? backendRole);
}
