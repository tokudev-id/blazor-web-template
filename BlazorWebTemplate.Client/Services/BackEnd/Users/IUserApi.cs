using BlazorWebTemplate.Shared.Common;

namespace BlazorWebTemplate.Client.Services.BackEnd.Users;

internal interface IUserApi
{
    Task<ApiResult<DummyJsonUserListResponseDto>> GetUsersAsync(string? search, CancellationToken cancellationToken);
}
