using BlazorWebTemplate.Client.Services.BackEnd.Rbac;
using BlazorWebTemplate.Shared.Services.Authorization.Models;
using BlazorWebTemplate.Web.Common.Components;
using BlazorWebTemplate.Web.Features.Rbac.Constants;
using BlazorWebTemplate.Web.Services.Shell;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorWebTemplate.Web.Features.Rbac.Pages;

public partial class Roles
{
    [Inject]
    private IRbacService RbacService { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    protected IReadOnlyList<AppShellBreadcrumb> _breadcrumbs = BreadcrumbFor.Index();

    protected List<string> _permissions = [];
    protected bool _permissionsLoading = true;
    protected string? _permissionsError;

    protected List<RoleSummary> _roles = [];
    protected bool _rolesLoading = true;
    protected string? _rolesError;
    protected readonly HashSet<string> _deletingRoleId = [];

    protected string _roleName = string.Empty;
    protected readonly HashSet<string> _selectedPermissions = [];
    protected bool _creating;
    protected string? _createError;

    protected override async Task OnInitializedAsync()
    {
        var permissionsTask = RbacService.GetPermissionsAsync();
        var rolesTask = RbacService.GetRolesAsync();

        await Task.WhenAll(permissionsTask, rolesTask);

        var permResult = permissionsTask.Result;
        _permissionsLoading = false;
        if (permResult.IsFailure)
            _permissionsError = permResult.Error?.Message ?? "Could not load permissions.";
        else
            _permissions = permResult.Value ?? [];

        var rolesResult = rolesTask.Result;
        _rolesLoading = false;
        if (rolesResult.IsFailure)
            _rolesError = rolesResult.Error?.Message ?? "Could not load roles.";
        else
            _roles = rolesResult.Value ?? [];
    }

    protected void TogglePermission(string permission)
    {
        if (!_selectedPermissions.Remove(permission))
            _selectedPermissions.Add(permission);
    }

    protected async Task CreateRoleAsync()
    {
        _createError = null;
        _creating = true;

        var result = await RbacService.CreateRoleAsync(_roleName.Trim(), _selectedPermissions);
        _creating = false;

        if (result.IsFailure)
        {
            _createError = result.Error?.Message ?? "Failed to create role.";
            return;
        }

        var name = _roleName.Trim();
        Snackbar.Add($"Role \"{name}\" created successfully.", Severity.Success);
        _roleName = string.Empty;
        _selectedPermissions.Clear();

        await RefreshRolesAsync();
    }

    protected async Task DeleteRoleAsync(string roleId)
    {
        var role = _roles.Find(r => r.Id == roleId);
        var roleName = role?.Name ?? roleId;

        var parameters = new DialogParameters<ConfirmDialog>
        {
            { x => x.Title, $"Delete role \"{roleName}\"?" },
            { x => x.Message, $"This will permanently remove the \"{roleName}\" role and all its permission assignments." },
            { x => x.WarningMessage, "Users with this role will lose associated permissions." },
        };

        var options = new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<ConfirmDialog>("Confirm delete", parameters, options);
        var dialogResult = await dialog.Result;

        if (dialogResult.Canceled) return;

        _deletingRoleId.Add(roleId);

        var result = await RbacService.DeleteRoleAsync(roleId);
        _deletingRoleId.Remove(roleId);

        if (result.IsFailure)
        {
            Snackbar.Add(result.Error?.Message ?? "Failed to delete role.", Severity.Error);
            return;
        }

        _roles.RemoveAll(r => r.Id == roleId);
        Snackbar.Add($"Role \"{roleName}\" deleted.", Severity.Success);
    }

    private async Task RefreshRolesAsync()
    {
        var result = await RbacService.GetRolesAsync();
        if (result.IsSuccess)
            _roles = result.Value ?? [];
    }
}
