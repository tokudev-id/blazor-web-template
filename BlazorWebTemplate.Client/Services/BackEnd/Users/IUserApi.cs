using BlazorWebTemplate.Shared.Common.Responses;

namespace BlazorWebTemplate.Client.Services.BackEnd.Users;

internal interface IUserApi
{
    Task<ApiResult<UnictiveUserListResponseDto>> GetUsersAsync(string? search, CancellationToken cancellationToken);
}
