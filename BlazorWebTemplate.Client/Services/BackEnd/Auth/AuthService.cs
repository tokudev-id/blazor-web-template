using BlazorWebTemplate.Client.Services.BackEnd.Infrastructure.Session;
using BlazorWebTemplate.Shared.Common.Constants;
using BlazorWebTemplate.Shared.Common.Responses;
using BlazorWebTemplate.Shared.Services.Authentication.Commands.Login;
using BlazorWebTemplate.Shared.Services.Authentication.Models;
using BlazorWebTemplate.Shared.Services.Authorization.Constants;

namespace BlazorWebTemplate.Client.Services.BackEnd.Auth;

internal sealed class AuthService(
    IAuthApi authApiClient,
    ITokenStore tokenStore) : IAuthService
{
    public async Task<ApiResult<AppUser>> LoginAsync(LoginCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Email) || string.IsNullOrWhiteSpace(command.Password))
        {
            return ApiResult<AppUser>.Failure(new ApiError(ApiErrorCodes.Validation, "Email and password are required."));
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
        var expiresAt = new DateTimeOffset(loginResult.Value.ExpiresAt, TimeSpan.Zero);
        var session = new AccessSession(loginResult.Value.AccessToken, loginResult.Value.RefreshToken, expiresAt, user);

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

    private static AppUser MapUser(UnictiveCurrentUserDto dto)
    {
        var displayName = string.IsNullOrWhiteSpace(dto.FullName)
            ? string.Join(' ', new[] { dto.FirstName, dto.LastName }.Where(static p => !string.IsNullOrWhiteSpace(p)))
            : dto.FullName;

        return new AppUser(
            dto.Id,
            dto.Email,
            string.IsNullOrWhiteSpace(displayName) ? dto.Email : displayName,
            dto.Email,
            dto.Roles.FirstOrDefault() ?? RoleNameFor.Viewer,
            null);
    }
}
