using System.Globalization;
using System.Security.Claims;
using BlazorWebTemplate.Shared.Services.Authentication.Constants;
using BlazorWebTemplate.Shared.Services.Authentication.Models;
using BlazorWebTemplate.Shared.Services.Authorization.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace BlazorWebTemplate.Client.Services.BackEnd.Infrastructure.Session;

internal sealed class CookieAuthenticationTokenStore(IHttpContextAccessor httpContextAccessor) : ITokenStore
{
    public async Task<AccessSession?> GetSessionAsync(CancellationToken cancellationToken = default)
    {
        var authenticateResult = await AuthenticateAsync();
        if (authenticateResult?.Succeeded != true || authenticateResult.Principal?.Identity?.IsAuthenticated != true)
        {
            return null;
        }

        var accessToken = authenticateResult.Properties?.GetTokenValue(AuthConstants.AccessTokenName);
        var refreshToken = authenticateResult.Properties?.GetTokenValue(AuthConstants.RefreshTokenName);
        var expiresAtRaw = authenticateResult.Properties?.GetTokenValue(AuthConstants.ExpiresAtTokenName);

        if (string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrWhiteSpace(refreshToken) || string.IsNullOrWhiteSpace(expiresAtRaw))
        {
            return null;
        }

        if (!DateTimeOffset.TryParse(expiresAtRaw, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var expiresAtUtc))
        {
            return null;
        }

        var principal = authenticateResult.Principal;
        var user = new AppUser(
            principal.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty,
            principal.FindFirstValue(ClaimTypes.Name) ?? string.Empty,
            principal.FindFirstValue("display_name") ?? string.Empty,
            principal.FindFirstValue(ClaimTypes.Email) ?? string.Empty,
            principal.FindFirstValue(ClaimTypes.Role) ?? RoleNameFor.Viewer,
            principal.FindFirstValue("avatar_url"));

        return new AccessSession(accessToken, refreshToken, expiresAtUtc, user);
    }

    public async Task<string?> GetAccessTokenAsync(CancellationToken cancellationToken = default)
        => (await AuthenticateAsync())?.Properties?.GetTokenValue(AuthConstants.AccessTokenName);

    public async Task<string?> GetRefreshTokenAsync(CancellationToken cancellationToken = default)
        => (await AuthenticateAsync())?.Properties?.GetTokenValue(AuthConstants.RefreshTokenName);

    public async Task StoreAsync(AccessSession session, CancellationToken cancellationToken = default)
    {
        var httpContext = GetHttpContext();
        var principal = CreatePrincipal(session.User);
        var properties = CreateProperties(session);

        await httpContext.SignInAsync(AuthConstants.CookieScheme, principal, properties);
    }

    public async Task RefreshAsync(string accessToken, string refreshToken, DateTimeOffset expiresAtUtc, CancellationToken cancellationToken = default)
    {
        var authenticateResult = await AuthenticateAsync();
        if (authenticateResult?.Principal is null)
        {
            return;
        }

        var existingProperties = authenticateResult.Properties ?? new AuthenticationProperties();
        existingProperties.StoreTokens(CreateTokenSet(accessToken, refreshToken, expiresAtUtc));
        existingProperties.ExpiresUtc = expiresAtUtc.AddHours(8);

        await GetHttpContext().SignInAsync(AuthConstants.CookieScheme, authenticateResult.Principal, existingProperties);
    }

    public async Task ClearAsync(CancellationToken cancellationToken = default)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext is null)
        {
            return;
        }

        await httpContext.SignOutAsync(AuthConstants.CookieScheme);
    }

    private async Task<AuthenticateResult?> AuthenticateAsync()
    {
        var httpContext = httpContextAccessor.HttpContext;
        return httpContext is null
            ? null
            : await httpContext.AuthenticateAsync(AuthConstants.CookieScheme);
    }

    private static ClaimsPrincipal CreatePrincipal(AppUser user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role),
            new("display_name", user.DisplayName),
        };

        if (!string.IsNullOrWhiteSpace(user.AvatarUrl))
        {
            claims.Add(new Claim("avatar_url", user.AvatarUrl));
        }

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        return new ClaimsPrincipal(identity);
    }

    private static AuthenticationProperties CreateProperties(AccessSession session)
    {
        var properties = new AuthenticationProperties
        {
            AllowRefresh = true,
            ExpiresUtc = session.ExpiresAtUtc.AddHours(8),
            IsPersistent = false,
        };

        properties.StoreTokens(CreateTokenSet(session.AccessToken, session.RefreshToken, session.ExpiresAtUtc));
        return properties;
    }

    private static IEnumerable<AuthenticationToken> CreateTokenSet(string accessToken, string refreshToken, DateTimeOffset expiresAtUtc)
    {
        return
        [
            new AuthenticationToken { Name = AuthConstants.AccessTokenName, Value = accessToken },
            new AuthenticationToken { Name = AuthConstants.RefreshTokenName, Value = refreshToken },
            new AuthenticationToken { Name = AuthConstants.ExpiresAtTokenName, Value = expiresAtUtc.ToString("O", CultureInfo.InvariantCulture) },
        ];
    }

    private HttpContext GetHttpContext()
        => httpContextAccessor.HttpContext ?? throw new InvalidOperationException("An active HTTP context is required for authentication operations.");

}
