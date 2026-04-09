using BlazorWebTemplate.Client.Services.BackEnd.Infrastructure.Mapping;
using BlazorWebTemplate.Shared.Common.Constants;
using BlazorWebTemplate.Shared.Common.Responses;
using BlazorWebTemplate.Shared.Users.Queries.GetUsers;

namespace BlazorWebTemplate.Client.Services.BackEnd.Users;

internal sealed class UserService(
    IUserApi usersApiClient,
    IUserRoleMapper roleMapper) : IUserService
{
    public async Task<ApiResult<PagedResult<UserSummary>>> GetUsersAsync(string? search = null, int pageNumber = 1, CancellationToken cancellationToken = default)
    {
        var result = await usersApiClient.GetUsersAsync(search, pageNumber, cancellationToken);
        if (result.IsFailure || result.Value is null)
            return ApiResult<PagedResult<UserSummary>>.Failure(result.Error ?? new ApiError(ApiErrorCodes.Unknown, "Unable to load users."));

        var items = result.Value.Items
            .Select(MapToSummary)
            .ToList();

        var paged = new PagedResult<UserSummary>(
            items,
            result.Value.PageNumber,
            result.Value.PageSize,
            result.Value.TotalCount);

        return ApiResult<PagedResult<UserSummary>>.Success(paged);
    }

    public async Task<ApiResult<UserSummary>> UpdateUserAsync(string userId, UserUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var dto = new UnictiveUpdateUserRequestDto
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
        };

        var result = await usersApiClient.UpdateUserAsync(userId, dto, cancellationToken);
        if (result.IsFailure || result.Value is null)
            return ApiResult<UserSummary>.Failure(result.Error ?? new ApiError(ApiErrorCodes.Unknown, "Unable to update user."));

        return ApiResult<UserSummary>.Success(MapToSummary(result.Value));
    }

    public Task<ApiResult> DeleteUserAsync(string userId, CancellationToken cancellationToken = default)
        => usersApiClient.DeleteUserAsync(userId, cancellationToken);

    private UserSummary MapToSummary(UnictiveUserDto user) => new(
        user.Id,
        string.IsNullOrWhiteSpace(user.FullName)
            ? string.Join(' ', new[] { user.FirstName, user.LastName }.Where(static v => !string.IsNullOrWhiteSpace(v)))
            : user.FullName,
        user.Email,
        roleMapper.MapToAppRole(user.Roles.FirstOrDefault()),
        user.IsActive,
        user.FirstName,
        user.LastName);
}
