using BlazorWebTemplate.Client.Services.BackEnd.Rbac;
using BlazorWebTemplate.Client.Services.BackEnd.Users;
using BlazorWebTemplate.Shared.Services.Authorization.Constants;
using BlazorWebTemplate.Shared.Users.Queries.GetUsers;
using BlazorWebTemplate.Web.Common.Components;
using BlazorWebTemplate.Web.Features.Rbac.Components;
using BlazorWebTemplate.Web.Features.Users.Components;
using BlazorWebTemplate.Web.Features.Users.Constants;
using BlazorWebTemplate.Web.Services.Shell;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using InviteUserDialog = BlazorWebTemplate.Web.Features.Users.Components.InviteUserDialog;

namespace BlazorWebTemplate.Web.Features.Users.Pages;

public partial class Users
{
    [Inject]
    private IUserService UserService { get; set; } = default!;

    [Inject]
    private IRbacService RbacService { get; set; } = default!;

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

    // roleName → list of permissions (loaded once on init)
    private Dictionary<string, IReadOnlyList<string>> _rolePermissions = [];

    protected override async Task OnInitializedAsync()
    {
        var rolesResult = await RbacService.GetRolesAsync();
        if (rolesResult.IsSuccess && rolesResult.Value is not null)
        {
            _rolePermissions = rolesResult.Value.ToDictionary(
                r => r.Name,
                r => r.Permissions,
                StringComparer.OrdinalIgnoreCase);
        }
    }

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

    protected IReadOnlyList<string> GetUserPermissions(IReadOnlyList<string> roles)
        => roles
            .SelectMany(r => _rolePermissions.TryGetValue(r, out var perms) ? perms : [])
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Order()
            .ToList();

    protected void ApplySearch()
        => NavigationManager.NavigateTo(RouteFor.WithSearch(_searchInput), forceLoad: false);

    protected void ClearSearch()
        => NavigationManager.NavigateTo(RouteFor.Index, forceLoad: false);

    protected void OnPageChanged(int page)
        => NavigationManager.NavigateTo(RouteFor.WithSearchAndPage(_searchInput, page), forceLoad: false);

    protected async Task OpenInviteUserDialogAsync()
    {
        var options = new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<InviteUserDialog>("Add new user", options);
        var result = await dialog.Result;

        if (result is { Canceled: false, Data: string email })
        {
            Snackbar.Add($"User {email} created successfully.", Severity.Success);
            NavigationManager.NavigateTo(RouteFor.Index, forceLoad: false);
        }
    }

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

        if (result is { Canceled: false, Data: string assignedRole })
        {
            var idx = _users.FindIndex(u => u.Id == user.Id);
            if (idx >= 0)
            {
                var updatedRoles = _users[idx].Roles
                    .Append(assignedRole)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();
                _users[idx] = _users[idx] with { Roles = updatedRoles };
            }
            Snackbar.Add($"Role \"{assignedRole}\" assigned to {user.DisplayName}.", Severity.Success);
            StateHasChanged();
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

        if (result is { Canceled: false, Data: UserSummary updated })
        {
            var idx = _users.FindIndex(u => u.Id == updated.Id);
            if (idx >= 0) _users[idx] = updated;
            Snackbar.Add($"{updated.DisplayName} updated.", Severity.Success);
            StateHasChanged();
        }
    }

    protected async Task ToggleUserActiveAsync(UserSummary user)
    {
        var activate = !user.IsActive;
        var action = activate ? "Activate" : "Deactivate";
        var consequence = activate
            ? $"This will restore {user.DisplayName}'s access to the system."
            : $"This will disable {user.DisplayName}'s access to the system.";

        var parameters = new DialogParameters<ConfirmDialog>
        {
            { x => x.Title, $"{action} {user.DisplayName}?" },
            { x => x.Message, consequence },
            { x => x.ConfirmLabel, action },
        };

        var options = new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<ConfirmDialog>($"{action} user", parameters, options);
        var dialogResult = await dialog.Result;

        if (dialogResult is null || dialogResult.Canceled) return;

        var result = await UserService.SetUserActiveAsync(user.Id, activate);

        if (result.IsFailure)
        {
            Snackbar.Add(result.Error?.Message ?? $"Failed to {action.ToLowerInvariant()} user.", Severity.Error);
            return;
        }

        var idx = _users.FindIndex(u => u.Id == user.Id);
        if (idx >= 0) _users[idx] = _users[idx] with { IsActive = activate };
        Snackbar.Add($"{user.DisplayName} {(activate ? "activated" : "deactivated")}.", Severity.Success);
        StateHasChanged();
    }

    protected static Color RoleColor(string? role) => role?.ToLowerInvariant() switch
    {
        "admin" => Color.Primary,
        "moderator" or "editor" => Color.Info,
        "viewer" => Color.Default,
        _ => Color.Secondary
    };

    protected static string GetInitials(string? name)
    {
        if (string.IsNullOrWhiteSpace(name)) return "?";
        var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length >= 2
            ? $"{parts[0][0]}{parts[^1][0]}".ToUpperInvariant()
            : name[0].ToString().ToUpperInvariant();
    }

    protected static string AvatarColor(string? name)
    {
        if (string.IsNullOrWhiteSpace(name)) return "#1f4fd8";
        string[] colors = ["#1f4fd8", "#00695e", "#7c3aed", "#dc2626", "#d97706", "#0891b2", "#16a34a", "#db2777"];
        return colors[Math.Abs(name[0]) % colors.Length];
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

        if (dialogResult is null || dialogResult.Canceled) return;

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
