using BlazorWebTemplate.Backend.Mapping;
using BlazorWebTemplate.Shared.Common;
using BlazorWebTemplate.Shared.Users;

namespace BlazorWebTemplate.Backend.Users;

internal sealed class UserService(
    DummyJsonUsersApiClient usersApiClient,
    IUserRoleMapper roleMapper) : IUserService
{
    public async Task<ApiResult<IReadOnlyList<UserSummary>>> GetUsersAsync(string? search = null, CancellationToken cancellationToken = default)
    {
        var result = await usersApiClient.GetUsersAsync(search, cancellationToken);
        if (result.IsFailure || result.Value is null)
        {
            return ApiResult<IReadOnlyList<UserSummary>>.Failure(result.Error ?? new ApiError(ApiErrorCodes.Unknown, "Unable to load users."));
        }

        var users = result.Value.Users
            .Select(user => new UserSummary(
                user.Id,
                string.Join(' ', new[] { user.FirstName, user.LastName }.Where(static value => !string.IsNullOrWhiteSpace(value))),
                user.Email,
                user.Username,
                roleMapper.MapToAppRole(user.Role),
                user.Company?.Name ?? "Independent",
                user.Image))
            .Cast<UserSummary>()
            .ToList();

        return ApiResult<IReadOnlyList<UserSummary>>.Success(users);
    }
}
