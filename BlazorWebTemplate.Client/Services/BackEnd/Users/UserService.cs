using BlazorWebTemplate.Client.Services.BackEnd.Infrastructure.Mapping;
using BlazorWebTemplate.Shared.Common.Constants;
using BlazorWebTemplate.Shared.Common.Responses;
using BlazorWebTemplate.Shared.Users.Queries.GetUsers;

namespace BlazorWebTemplate.Client.Services.BackEnd.Users;

internal sealed class UserService(
    IUserApi usersApiClient,
    IUserRoleMapper roleMapper) : IUserService
{
    public async Task<ApiResult<IReadOnlyList<UserSummary>>> GetUsersAsync(string? search = null, CancellationToken cancellationToken = default)
    {
        var result = await usersApiClient.GetUsersAsync(search, cancellationToken);
        if (result.IsFailure || result.Value is null)
        {
            return ApiResult<IReadOnlyList<UserSummary>>.Failure(result.Error ?? new ApiError(ApiErrorCodes.Unknown, "Unable to load users."));
        }

        var users = result.Value.Items
            .Select(user => new UserSummary(
                user.Id,
                string.IsNullOrWhiteSpace(user.FullName)
                    ? string.Join(' ', new[] { user.FirstName, user.LastName }.Where(static v => !string.IsNullOrWhiteSpace(v)))
                    : user.FullName,
                user.Email,
                roleMapper.MapToAppRole(user.Roles.FirstOrDefault()),
                user.IsActive))
            .ToList();

        return ApiResult<IReadOnlyList<UserSummary>>.Success(users);
    }
}
