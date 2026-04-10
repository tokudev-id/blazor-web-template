using BlazorWebTemplate.Shared.Common.Responses;

namespace BlazorWebTemplate.Client.Services.BackEnd.Users;

internal interface IUserApi
{
    Task<ApiResult<UnictiveUserListResponseDto>> GetUsersAsync(string? search, int pageNumber, CancellationToken cancellationToken);
    Task<ApiResult<UnictiveUserDto>> GetUserAsync(string userId, CancellationToken cancellationToken);
    Task<ApiResult<UnictiveUserDto>> UpdateUserAsync(string userId, UnictiveUpdateUserRequestDto request, CancellationToken cancellationToken);
    Task<ApiResult> DeleteUserAsync(string userId, CancellationToken cancellationToken);
    Task<ApiResult> RegisterUserAsync(UnictiveRegisterUserRequestDto request, CancellationToken cancellationToken);
    Task<ApiResult> ActivateUserAsync(string userId, CancellationToken cancellationToken);
    Task<ApiResult> DeactivateUserAsync(string userId, CancellationToken cancellationToken);
}
