using BlazorWebTemplate.Backend.Mapping;
using BlazorWebTemplate.Backend.Session;
using BlazorWebTemplate.Shared.Auth;
using BlazorWebTemplate.Shared.Common;

namespace BlazorWebTemplate.Backend.Auth;

internal sealed class AuthService(
    DummyJsonAuthApiClient authApiClient,
    ITokenStore tokenStore,
    IUserRoleMapper roleMapper) : IAuthService
{
    public async Task<ApiResult<AppUser>> LoginAsync(LoginCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Username) || string.IsNullOrWhiteSpace(command.Password))
        {
            return ApiResult<AppUser>.Failure(new ApiError(ApiErrorCodes.Validation, "Username and password are required."));
        }

        var loginResult = await authApiClient.LoginAsync(command, cancellationToken);
        if (loginResult.IsFailure || loginResult.Value is null)
        {
            return ApiResult<AppUser>.Failure(loginResult.Error ?? new ApiError(ApiErrorCodes.Unknown, "Unable to sign in."));
        }

        var meResult = await authApiClient.GetCurrentUserAsync(loginResult.Value.AccessToken, cancellationToken);
        if (meResult.IsFailure || meResult.Value is null)
        {
            return ApiResult<AppUser>.Failure(meResult.Error ?? new ApiError(ApiErrorCodes.Unknown, "Unable to load the current user profile."));
        }

        var user = MapUser(meResult.Value);
        var session = new AccessSession(
            loginResult.Value.AccessToken,
            loginResult.Value.RefreshToken,
            DateTimeOffset.UtcNow.AddMinutes(30),
            user);

        await tokenStore.StoreAsync(session, cancellationToken);
        return ApiResult<AppUser>.Success(user);
    }

    public async Task<ApiResult<AppUser>> GetCurrentUserAsync(CancellationToken cancellationToken = default)
    {
        var session = await tokenStore.GetSessionAsync(cancellationToken);
        return session is null
            ? ApiResult<AppUser>.Failure(new ApiError(ApiErrorCodes.Unauthorized, "No authenticated session was found."))
            : ApiResult<AppUser>.Success(session.User);
    }

    public Task LogoutAsync(CancellationToken cancellationToken = default)
        => tokenStore.ClearAsync(cancellationToken);

    private AppUser MapUser(DummyJsonCurrentUserDto dto)
    {
        var displayName = string.Join(' ', new[] { dto.FirstName, dto.LastName }.Where(static part => !string.IsNullOrWhiteSpace(part)));
        return new AppUser(
            dto.Id,
            dto.Username,
            string.IsNullOrWhiteSpace(displayName) ? dto.Username : displayName,
            dto.Email,
            roleMapper.MapToAppRole(dto.Role),
            dto.Image);
    }
}
