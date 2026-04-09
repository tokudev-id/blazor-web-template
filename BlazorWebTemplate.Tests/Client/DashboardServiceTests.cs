using BlazorWebTemplate.Client.Services.BackEnd.Auth;
using BlazorWebTemplate.Client.Services.BackEnd.Dashboard;
using BlazorWebTemplate.Client.Services.BackEnd.Users;
using BlazorWebTemplate.Shared.Common.Responses;
using BlazorWebTemplate.Shared.Dashboard.Queries.GetDashboard;
using BlazorWebTemplate.Shared.Services.Authentication.Models;
using BlazorWebTemplate.Shared.Services.Authorization.Constants;
using BlazorWebTemplate.Shared.Users.Queries.GetUsers;

namespace BlazorWebTemplate.Tests.Client;

public sealed class DashboardServiceTests
{
    [Fact]
    public async Task GetSummaryAsync_ComposesCountsAndCurrentRole()
    {
        var service = new DashboardService(
            new FakeUserService(),
            new FakeCurrentUserService());

        var result = await service.GetSummaryAsync();

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(3, result.Value!.TotalUsers);
        Assert.Equal(RoleNameFor.Admin, result.Value.MyRole);
    }

    private sealed class FakeUserService : IUserService
    {
        public Task<ApiResult<IReadOnlyList<UserSummary>>> GetUsersAsync(string? search = null, CancellationToken cancellationToken = default)
            => Task.FromResult(ApiResult<IReadOnlyList<UserSummary>>.Success(
                [
                    new UserSummary("id-1", "A", "a@example.com", RoleNameFor.Admin, true),
                    new UserSummary("id-2", "B", "b@example.com", RoleNameFor.Editor, true),
                    new UserSummary("id-3", "C", "c@example.com", RoleNameFor.Viewer, false)
                ]));
    }

    private sealed class FakeCurrentUserService : ICurrentUserService
    {
        public Task<AppUser?> GetCurrentUserAsync(CancellationToken cancellationToken = default)
            => Task.FromResult<AppUser?>(new AppUser("guid-admin", "admin@example.com", "Admin", "admin@example.com", RoleNameFor.Admin, null));
    }
}
