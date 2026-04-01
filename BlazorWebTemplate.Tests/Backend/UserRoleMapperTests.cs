using BlazorWebTemplate.Backend.Mapping;
using BlazorWebTemplate.Shared.Auth;

namespace BlazorWebTemplate.Tests.Backend;

public sealed class UserRoleMapperTests
{
    private readonly UserRoleMapper _mapper = new();

    [Theory]
    [InlineData("admin", RoleNames.Admin)]
    [InlineData("moderator", RoleNames.Editor)]
    [InlineData("user", RoleNames.Viewer)]
    [InlineData(null, RoleNames.Viewer)]
    public void MapToAppRole_ReturnsExpectedRole(string? backendRole, string expectedRole)
    {
        var mappedRole = _mapper.MapToAppRole(backendRole);

        Assert.Equal(expectedRole, mappedRole);
    }
}
