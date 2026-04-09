using BlazorWebTemplate.Shared.Common.Responses;
using BlazorWebTemplate.Shared.Users.Queries.GetUsers;

namespace BlazorWebTemplate.Client.Services.BackEnd.Users;

public interface IUserService
{
    Task<ApiResult<PagedResult<UserSummary>>> GetUsersAsync(string? search = null, int pageNumber = 1, CancellationToken cancellationToken = default);
    Task<ApiResult<UserSummary>> UpdateUserAsync(string userId, UserUpdateRequest request, CancellationToken cancellationToken = default);
    Task<ApiResult> DeleteUserAsync(string userId, CancellationToken cancellationToken = default);
}
