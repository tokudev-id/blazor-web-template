using BlazorWebTemplate.Shared.Common;
using BlazorWebTemplate.Shared.Users;

namespace BlazorWebTemplate.Client.Services.BackEnd.Users;

public interface IUserService
{
    Task<ApiResult<IReadOnlyList<UserSummary>>> GetUsersAsync(string? search = null, CancellationToken cancellationToken = default);
}
