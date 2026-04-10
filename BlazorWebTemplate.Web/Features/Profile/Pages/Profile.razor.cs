using BlazorWebTemplate.Client.Services.BackEnd.Auth;
using BlazorWebTemplate.Shared.Users.Queries.GetUsers;
using BlazorWebTemplate.Web.Features.Profile.Constants;
using BlazorWebTemplate.Web.Services.Shell;
using Microsoft.AspNetCore.Components;

namespace BlazorWebTemplate.Web.Features.Profile.Pages;

public partial class Profile
{
    [Inject]
    private IProfileService ProfileService { get; set; } = default!;

    protected IReadOnlyList<AppShellBreadcrumb> _breadcrumbs = BreadcrumbFor.Index();
    protected bool _isLoading = true;

    protected string _email = string.Empty;
    protected string? _firstName;
    protected string? _lastName;
    protected string? _phoneNumber;
    protected bool _savingProfile;
    protected string? _profileError;
    protected string? _profileSuccess;

    protected override async Task OnInitializedAsync()
    {
        var result = await ProfileService.GetProfileAsync();
        _isLoading = false;

        if (result.IsSuccess && result.Value is not null)
        {
            _email = result.Value.Email;
            _firstName = result.Value.DisplayName.Split(' ', 2).ElementAtOrDefault(0);
            _lastName = result.Value.DisplayName.Split(' ', 2).ElementAtOrDefault(1);
        }
    }

    protected async Task SaveProfileAsync()
    {
        _profileError = null;
        _profileSuccess = null;
        _savingProfile = true;

        var result = await ProfileService.UpdateProfileAsync(new UserUpdateRequest(_firstName, _lastName, _phoneNumber));
        _savingProfile = false;

        if (result.IsFailure)
        {
            _profileError = result.Error?.Message ?? "Failed to save profile.";
            return;
        }

        _profileSuccess = "Profile updated successfully.";
    }
}
