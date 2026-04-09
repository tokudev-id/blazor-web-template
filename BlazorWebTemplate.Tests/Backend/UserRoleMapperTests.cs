using BlazorWebTemplate.Client.Services.BackEnd.Infrastructure.Mapping;
using BlazorWebTemplate.Shared.Services.Authorization.Constants;

namespace BlazorWebTemplate.Tests.Backend;

public sealed class UserRoleMapperTests
{
    private readonly UserRoleMapper _mapper = new();

    [Theory]
    [InlineData("admin", RoleNameFor.Admin)]
    [InlineData("moderator", RoleNameFor.Editor)]
    [InlineData("user", RoleNameFor.Viewer)]
    [InlineData(null, RoleNameFor.Viewer)]
    public void MapToAppRole_ReturnsExpectedRole(string? backendRole, string expectedRole)
    {
        var mappedRole = _mapper.MapToAppRole(backendRole);

        Assert.Equal(expectedRole, mappedRole);
    }
}
