using BlazorWebTemplate.Client.Services.BackEnd.Users;
using BlazorWebTemplate.Shared.Users.Queries.GetUsers;
using BlazorWebTemplate.Web.Common.Components;
using BlazorWebTemplate.Web.Features.Rbac.Components;
using BlazorWebTemplate.Web.Features.Users.Components;
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

    [SupplyParameterFromQuery(Name = "page")]
    public int? Page { get; set; }

    protected IReadOnlyList<AppShellBreadcrumb> _breadcrumbs = BreadcrumbFor.Index();
    protected List<UserSummary> _users = [];
    protected bool _isLoading = true;
    protected string? _errorMessage;
    protected string _searchInput = string.Empty;
    protected int _currentPage = 1;
    protected int _totalPages = 1;
    protected int _totalCount;
    protected readonly HashSet<string> _deletingUserId = [];

    protected override async Task OnParametersSetAsync()
    {
        _isLoading = true;
        _searchInput = Search ?? string.Empty;
        _currentPage = Page is > 0 ? Page.Value : 1;

        var result = await UserService.GetUsersAsync(Search, _currentPage);
        _isLoading = false;

        if (result.IsFailure)
        {
            _errorMessage = result.Error?.Message ?? "The users could not be loaded.";
            _users = [];
            return;
        }

        _errorMessage = null;
        var paged = result.Value!;
        _users = paged.Items.ToList();
        _totalPages = paged.TotalPages;
        _totalCount = paged.TotalCount;
        _currentPage = paged.PageNumber;
    }

    protected void ApplySearch()
        => NavigationManager.NavigateTo(RouteFor.WithSearch(_searchInput), forceLoad: false);

    protected void ClearSearch()
        => NavigationManager.NavigateTo(RouteFor.Index, forceLoad: false);

    protected void OnPageChanged(int page)
        => NavigationManager.NavigateTo(RouteFor.WithSearchAndPage(_searchInput, page), forceLoad: false);

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

    protected async Task OpenEditDialogAsync(UserSummary user)
    {
        var parameters = new DialogParameters<EditUserDialog>
        {
            { x => x.UserId, user.Id },
            { x => x.DisplayName, user.DisplayName },
            { x => x.InitialFirstName, user.FirstName },
            { x => x.InitialLastName, user.LastName },
        };

        var options = new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<EditUserDialog>("Edit user", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled && result.Data is UserSummary updated)
        {
            var idx = _users.FindIndex(u => u.Id == updated.Id);
            if (idx >= 0) _users[idx] = updated;
            Snackbar.Add($"{updated.DisplayName} updated.", Severity.Success);
        }
    }

    protected async Task DeleteUserAsync(UserSummary user)
    {
        var parameters = new DialogParameters<ConfirmDialog>
        {
            { x => x.Title, $"Delete {user.DisplayName}?" },
            { x => x.Message, $"This will permanently remove {user.DisplayName} ({user.Email}) from the system." },
            { x => x.WarningMessage, "This action cannot be undone." },
        };

        var options = new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<ConfirmDialog>("Confirm delete", parameters, options);
        var dialogResult = await dialog.Result;

        if (dialogResult.Canceled) return;

        _deletingUserId.Add(user.Id);

        var result = await UserService.DeleteUserAsync(user.Id);
        _deletingUserId.Remove(user.Id);

        if (result.IsFailure)
        {
            Snackbar.Add(result.Error?.Message ?? "Failed to delete user.", Severity.Error);
            return;
        }

        _users.RemoveAll(u => u.Id == user.Id);
        _totalCount--;
        Snackbar.Add($"{user.DisplayName} deleted.", Severity.Success);
    }
}
