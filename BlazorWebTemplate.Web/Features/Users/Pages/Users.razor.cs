using BlazorWebTemplate.Client.Services.BackEnd.Users;
using BlazorWebTemplate.Shared.Users;
using BlazorWebTemplate.Web.Common.Constants;
using BlazorWebTemplate.Web.Services.Shell;
using Microsoft.AspNetCore.Components;

namespace BlazorWebTemplate.Web.Features.Users.Pages;

public partial class Users
{
    [Inject]
    private IUserService UserService { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [SupplyParameterFromQuery(Name = "search")]
    public string? Search { get; set; }

    protected IReadOnlyList<AppShellBreadcrumb> _breadcrumbs = AppBreadcrumbs.Users();
    protected List<UserSummary> _users = [];
    protected bool _isLoading = true;
    protected string? _errorMessage;
    protected string _searchInput = string.Empty;

    protected override async Task OnParametersSetAsync()
    {
        _isLoading = true;
        _searchInput = Search ?? string.Empty;
        var result = await UserService.GetUsersAsync(Search);
        _isLoading = false;

        if (result.IsFailure)
        {
            _errorMessage = result.Error?.Message ?? "The users could not be loaded.";
            _users = [];
            return;
        }

        _errorMessage = null;
        _users = result.Value?.ToList() ?? [];
    }

    protected void ApplySearch()
        => NavigationManager.NavigateTo(AppRoutes.UsersSearch(_searchInput), forceLoad: true);

    protected void ClearSearch()
        => NavigationManager.NavigateTo(AppRoutes.Users, forceLoad: true);
}
