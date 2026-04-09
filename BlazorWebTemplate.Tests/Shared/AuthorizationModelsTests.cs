using BlazorWebTemplate.Shared.Services.Authorization.Models;
using BlazorWebTemplate.Shared.Users.Queries.GetUsers;

namespace BlazorWebTemplate.Tests.Shared;

public sealed class AuthorizationModelsTests
{
    [Fact]
    public void RoleSummary_StoresProperties()
    {
        var perms = new[] { "users.read", "users.write" };
        var role = new RoleSummary("id-1", "Admin", perms);

        Assert.Equal("id-1", role.Id);
        Assert.Equal("Admin", role.Name);
        Assert.Equal(perms, role.Permissions);
    }

    [Fact]
    public void RoleSummary_EmptyPermissions_IsAllowed()
    {
        var role = new RoleSummary("id-2", "Viewer", []);
        Assert.Empty(role.Permissions);
    }

    [Fact]
    public void UserUpdateRequest_AllNullable_AllowsPartialUpdate()
    {
        var req = new UserUpdateRequest(null, "Smith", null);

        Assert.Null(req.FirstName);
        Assert.Equal("Smith", req.LastName);
        Assert.Null(req.PhoneNumber);
    }

    [Fact]
    public void UserUpdateRequest_RecordEquality_Works()
    {
        var a = new UserUpdateRequest("Jane", "Doe", "+1234");
        var b = new UserUpdateRequest("Jane", "Doe", "+1234");

        Assert.Equal(a, b);
    }
}
