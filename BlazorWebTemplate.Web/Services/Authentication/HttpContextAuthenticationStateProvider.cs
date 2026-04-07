using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorWebTemplate.Web.Services.Authentication;

public sealed class HttpContextAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor) : AuthenticationStateProvider
{
    private static readonly AuthenticationState Anonymous = new(new ClaimsPrincipal(new ClaimsIdentity()));

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = httpContextAccessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated == true)
        {
            return Task.FromResult(new AuthenticationState(user));
        }

        return Task.FromResult(Anonymous);
    }
}
