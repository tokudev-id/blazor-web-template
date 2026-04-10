using BlazorWebTemplate.Client.Services.BackEnd.Auth;
using BlazorWebTemplate.Client.Services.BackEnd.Dashboard;
using BlazorWebTemplate.Client.Services.BackEnd.Users;
using BlazorWebTemplate.Shared.Common.Responses;
using BlazorWebTemplate.Shared.Dashboard.Queries.GetDashboard;
using BlazorWebTemplate.Shared.Users.Queries.GetUsers;
using BlazorWebTemplate.Shared.Services.Authentication.Models;
using BlazorWebTemplate.Shared.Services.Authorization.Constants;

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
        Assert.Equal(2, result.Value.ActiveUsers);
        Assert.Equal(1, result.Value.InactiveUsers);
        Assert.Equal(RoleNameFor.Admin, result.Value.MyRole);
    }

    private sealed class FakeUserService : IUserService
    {
        public Task<ApiResult<PagedResult<UserSummary>>> GetUsersAsync(string? search = null, int pageNumber = 1, CancellationToken cancellationToken = default)
        {
            var items = new List<UserSummary>
            {
                new("id-1", "A", "a@example.com", [RoleNameFor.Admin], true),
                new("id-2", "B", "b@example.com", [RoleNameFor.Editor], true),
                new("id-3", "C", "c@example.com", [RoleNameFor.Viewer], false),
            };
            var paged = new PagedResult<UserSummary>(items, 1, 20, 3);
            return Task.FromResult(ApiResult<PagedResult<UserSummary>>.Success(paged));
        }

        public Task<ApiResult<UserSummary>> UpdateUserAsync(string userId, UserUpdateRequest request, CancellationToken cancellationToken = default)
            => throw new NotImplementedException();

        public Task<ApiResult> DeleteUserAsync(string userId, CancellationToken cancellationToken = default)
            => throw new NotImplementedException();

        public Task<ApiResult> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
            => throw new NotImplementedException();

        public Task<ApiResult> SetUserActiveAsync(string userId, bool activate, CancellationToken cancellationToken = default)
            => throw new NotImplementedException();
    }

    private sealed class FakeCurrentUserService : ICurrentUserService
    {
        public Task<AppUser?> GetCurrentUserAsync(CancellationToken cancellationToken = default)
            => Task.FromResult<AppUser?>(new AppUser("guid-admin", "admin@example.com", "Admin", "admin@example.com", RoleNameFor.Admin, null));
    }
}
