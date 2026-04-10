using BlazorWebTemplate.Client.Services.BackEnd.Rbac;
using BlazorWebTemplate.Web.Features.Rbac.Constants;
using BlazorWebTemplate.Web.Services.Shell;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorWebTemplate.Web.Features.Rbac.Pages;

public partial class RoleManage
{
    [Parameter]
    public string Id { get; set; } = default!;

    [Inject]
    private IRbacService RbacService { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected IReadOnlyList<AppShellBreadcrumb> _breadcrumbs = [];
    protected bool _isLoading = true;
    protected string? _loadError;

    protected string _roleName = string.Empty;
    protected string? _nameError;
    protected string? _nameSuccess;
    protected bool _savingName;

    protected List<string> _allPermissions = [];
    protected HashSet<string> _selectedPermissions = [];
    protected string? _permsError;
    protected string? _permsSuccess;
    protected bool _savingPerms;

    protected override async Task OnInitializedAsync()
    {
        var roleTask = RbacService.GetRoleAsync(Id);
        var permsTask = RbacService.GetPermissionsAsync();

        await Task.WhenAll(roleTask, permsTask);

        _isLoading = false;

        var roleResult = await roleTask;
        if (roleResult.IsFailure || roleResult.Value is null)
        {
            _loadError = roleResult.Error?.Message ?? "Could not load role.";
            return;
        }

        var role = roleResult.Value;
        _roleName = role.Name;
        _selectedPermissions = [.. role.Permissions];
        _breadcrumbs = BreadcrumbFor.Manage(role.Name, Id);

        var permsResult = await permsTask;
        if (permsResult.IsSuccess && permsResult.Value is not null)
            _allPermissions = permsResult.Value;
    }

    protected void TogglePermission(string perm)
    {
        if (!_selectedPermissions.Remove(perm))
            _selectedPermissions.Add(perm);
    }

    protected async Task SaveNameAsync()
    {
        _nameError = null;
        _nameSuccess = null;

        if (string.IsNullOrWhiteSpace(_roleName))
        {
            _nameError = "Role name is required.";
            return;
        }

        _savingName = true;
        var result = await RbacService.UpdateRoleAsync(Id, _roleName.Trim());
        _savingName = false;

        if (result.IsFailure)
        {
            _nameError = result.Error?.Message ?? "Failed to update role name.";
            return;
        }

        _nameSuccess = "Role name updated.";
        _breadcrumbs = BreadcrumbFor.Manage(_roleName, Id);
        Snackbar.Add("Role name updated.", Severity.Success);
    }

    protected async Task SavePermissionsAsync()
    {
        _permsError = null;
        _permsSuccess = null;

        _savingPerms = true;
        var result = await RbacService.UpdateRolePermissionsAsync(Id, _selectedPermissions);
        _savingPerms = false;

        if (result.IsFailure)
        {
            _permsError = result.Error?.Message ?? "Failed to update permissions.";
            return;
        }

        _permsSuccess = "Permissions updated.";
        Snackbar.Add("Permissions saved.", Severity.Success);
    }
}
