using BlazorWebTemplate.Client.Services.BackEnd.Auth;
using BlazorWebTemplate.Shared.Auth;
using BlazorWebTemplate.Web.Common.Constants;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWebTemplate.Web.Services.Authentication;

public static class AuthenticationEndpoints
{
    public static IEndpointRouteBuilder MapAuthenticationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(AppRoutes.AccountLogin, async (
            [FromForm] LoginCommand command,
            [FromForm] string? returnUrl,
            IAuthService authService) =>
        {
            var result = await authService.LoginAsync(command);

            if (result.IsFailure)
            {
                var message = result.Error?.Message ?? "The sign-in attempt could not be completed.";
                return Results.LocalRedirect(AppRoutes.LoginWithReturnUrl(returnUrl, message));
            }

            return Results.LocalRedirect(ResolveLocalReturnUrl(returnUrl));
        })
        .AllowAnonymous();

        endpoints.MapPost(AppRoutes.AccountLogout, async (IAuthService authService) =>
        {
            await authService.LogoutAsync();
            return Results.LocalRedirect(AppRoutes.Login);
        });

        return endpoints;
    }

    private static string ResolveLocalReturnUrl(string? returnUrl)
    {
        if (string.IsNullOrWhiteSpace(returnUrl))
        {
            return AppRoutes.Dashboard;
        }

        return returnUrl.StartsWith('/') && !returnUrl.StartsWith("//", StringComparison.Ordinal)
            ? returnUrl
            : AppRoutes.Dashboard;
    }
}
