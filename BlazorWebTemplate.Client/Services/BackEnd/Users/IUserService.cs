using BlazorWebTemplate.Shared.Common.Responses;
using BlazorWebTemplate.Shared.Users.Queries.GetUsers;

namespace BlazorWebTemplate.Client.Services.BackEnd.Users;

public interface IUserService
{
    Task<ApiResult<IReadOnlyList<UserSummary>>> GetUsersAsync(string? search = null, CancellationToken cancellationToken = default);
}
