namespace BlazorWebTemplate.Client.Services.BackEnd.Infrastructure.Mapping;

public interface IUserRoleMapper
{
    string MapToAppRole(string? backendRole);
}
