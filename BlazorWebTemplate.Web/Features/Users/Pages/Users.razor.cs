using BlazorWebTemplate.Client.Services.BackEnd.Users;
using BlazorWebTemplate.Shared.Users.Queries.GetUsers;
using BlazorWebTemplate.Web.Features.Rbac.Components;
using BlazorWebTemplate.Web.Features.Users.Constants;
using BlazorWebTemplate.Web.Services.Shell;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorWebTemplate.Web.Features.Users.Pages;

public partial class Users
{
    [Inject]
    private IUserService UserService { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    [SupplyParameterFromQuery(Name = "search")]
    public string? Search { get; set; }

    protected IReadOnlyList<AppShellBreadcrumb> _breadcrumbs = BreadcrumbFor.Index();
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
        => NavigationManager.NavigateTo(RouteFor.WithSearch(_searchInput), forceLoad: true);

    protected void ClearSearch()
        => NavigationManager.NavigateTo(RouteFor.Index, forceLoad: true);

    protected async Task OpenAssignRoleDialogAsync(UserSummary user)
    {
        var parameters = new DialogParameters<AssignRoleDialog>
        {
            { x => x.UserId, user.Id },
            { x => x.DisplayName, user.DisplayName },
        };

        var options = new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<AssignRoleDialog>("Assign role", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled && result.Data is string assignedRole)
        {
            Snackbar.Add($"Role \"{assignedRole}\" assigned to {user.DisplayName}.", Severity.Success);
        }
    }
}
