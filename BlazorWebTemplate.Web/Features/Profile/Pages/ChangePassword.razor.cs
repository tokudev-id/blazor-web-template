using System.Threading.Tasks;
using BlazorWebTemplate.Client.Services.BackEnd.Auth;
using BlazorWebTemplate.Shared.Services.Authentication.Commands.ChangePassword;
using BlazorWebTemplate.Web.Features.Profile.Constants;
using BlazorWebTemplate.Web.Services.Shell;
using Microsoft.AspNetCore.Components;

namespace BlazorWebTemplate.Web.Features.Profile.Pages;

public partial class ChangePassword
{
    [Inject]
    private IProfileService ProfileService { get; set; } = default!;

    protected IReadOnlyList<AppShellBreadcrumb> _breadcrumbs = BreadcrumbFor.ChangePassword();
    
    protected string _currentPassword = string.Empty;
    protected string _newPassword = string.Empty;
    protected string _confirmNewPassword = string.Empty;
    protected bool _savingPassword;
    protected string? _passwordError;
    protected string? _passwordSuccess;

    protected async Task ChangePasswordAsync()
    {
        _passwordError = null;
        _passwordSuccess = null;

        if (_newPassword != _confirmNewPassword)
        {
            _passwordError = "New password and confirmation do not match.";
            return;
        }

        if (_newPassword.Length < 6)
        {
            _passwordError = "New password must be at least 6 characters.";
            return;
        }

        _savingPassword = true;

        var request = new ChangePasswordRequest
        {
            CurrentPassword = _currentPassword,
            NewPassword = _newPassword,
            ConfirmNewPassword = _confirmNewPassword,
        };

        var result = await ProfileService.ChangePasswordAsync(request);
        _savingPassword = false;

        if (result.IsFailure)
        {
            _passwordError = result.Error?.Message ?? "Failed to change password.";
            return;
        }

        _passwordSuccess = "Password changed successfully.";
        _currentPassword = string.Empty;
        _newPassword = string.Empty;
        _confirmNewPassword = string.Empty;
    }
}
