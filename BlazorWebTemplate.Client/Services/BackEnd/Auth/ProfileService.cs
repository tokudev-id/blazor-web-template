using BlazorWebTemplate.Client.Services.BackEnd.Infrastructure.Mapping;
using BlazorWebTemplate.Shared.Common.Constants;
using BlazorWebTemplate.Shared.Common.Responses;
using BlazorWebTemplate.Shared.Services.Authentication.Commands.ChangePassword;
using BlazorWebTemplate.Shared.Services.Authentication.Models;
using BlazorWebTemplate.Shared.Users.Queries.GetUsers;

namespace BlazorWebTemplate.Client.Services.BackEnd.Auth;

internal sealed class ProfileService(
    IAuthApi authApi,
    ICurrentUserService currentUserService,
    IUserRoleMapper roleMapper) : IProfileService
{
    public async Task<ApiResult<AppUser>> GetProfileAsync(CancellationToken cancellationToken = default)
    {
        var user = await currentUserService.GetCurrentUserAsync(cancellationToken);
        if (user is null)
            return ApiResult<AppUser>.Failure(new ApiError(ApiErrorCodes.Unauthorized, "No active session found."));

        return ApiResult<AppUser>.Success(user);
    }

    public async Task<ApiResult<AppUser>> UpdateProfileAsync(UserUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var dto = new UnictiveUpdateProfileRequestDto
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
        };

        var result = await authApi.UpdateProfileAsync(dto, cancellationToken);
        if (result.IsFailure || result.Value is null)
            return ApiResult<AppUser>.Failure(result.Error ?? new ApiError(ApiErrorCodes.Unknown, "Unable to update profile."));

        return ApiResult<AppUser>.Success(MapToAppUser(result.Value));
    }

    public async Task<ApiResult> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken cancellationToken = default)
    {
        var dto = new UnictiveChangePasswordRequestDto
        {
            CurrentPassword = request.CurrentPassword,
            NewPassword = request.NewPassword,
            ConfirmNewPassword = request.ConfirmNewPassword,
        };

        return await authApi.ChangePasswordAsync(dto, cancellationToken);
    }

    private AppUser MapToAppUser(UnictiveCurrentUserDto dto) => new(
        dto.Id,
        dto.Email,
        string.IsNullOrWhiteSpace(dto.FullName)
            ? string.Join(' ', new[] { dto.FirstName, dto.LastName }.Where(static v => !string.IsNullOrWhiteSpace(v)))
            : dto.FullName,
        dto.Email,
        roleMapper.MapToAppRole(dto.Roles.FirstOrDefault()),
        null);
}
