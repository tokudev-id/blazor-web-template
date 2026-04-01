using BlazorWebTemplate.Shared.Common;
using BlazorWebTemplate.Shared.Users;

namespace BlazorWebTemplate.Backend.Users;

public interface IUserService
{
    Task<ApiResult<IReadOnlyList<UserSummary>>> GetUsersAsync(string? search = null, CancellationToken cancellationToken = default);
}
