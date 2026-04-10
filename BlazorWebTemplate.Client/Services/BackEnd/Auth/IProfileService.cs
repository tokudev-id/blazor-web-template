using BlazorWebTemplate.Shared.Common.Responses;
using BlazorWebTemplate.Shared.Services.Authentication.Commands.ChangePassword;
using BlazorWebTemplate.Shared.Services.Authentication.Models;
using BlazorWebTemplate.Shared.Users.Queries.GetUsers;

namespace BlazorWebTemplate.Client.Services.BackEnd.Auth;

public interface IProfileService
{
    Task<ApiResult<AppUser>> GetProfileAsync(CancellationToken cancellationToken = default);

    Task<ApiResult<AppUser>> UpdateProfileAsync(UserUpdateRequest request, CancellationToken cancellationToken = default);

    Task<ApiResult> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken cancellationToken = default);
}
