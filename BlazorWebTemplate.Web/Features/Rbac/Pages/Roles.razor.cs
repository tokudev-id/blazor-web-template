using BlazorWebTemplate.Client.Services.BackEnd.Rbac;
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

    protected IReadOnlyList<AppShellBreadcrumb> _breadcrumbs = BreadcrumbFor.Index();

    protected List<string> _permissions = [];
    protected bool _permissionsLoading = true;
    protected string? _permissionsError;

    protected string _roleName = string.Empty;
    protected readonly HashSet<string> _selectedPermissions = [];
    protected bool _creating;
    protected string? _createError;
    protected string? _createSuccess;

    protected override async Task OnInitializedAsync()
    {
        var result = await RbacService.GetPermissionsAsync();
        _permissionsLoading = false;

        if (result.IsFailure)
        {
            _permissionsError = result.Error?.Message ?? "Could not load permissions.";
            return;
        }

        _permissions = result.Value ?? [];
    }

    protected void TogglePermission(string permission)
    {
        if (!_selectedPermissions.Remove(permission))
            _selectedPermissions.Add(permission);
    }

    protected async Task CreateRoleAsync()
    {
        _createError = null;
        _createSuccess = null;
        _creating = true;

        var result = await RbacService.CreateRoleAsync(_roleName.Trim(), _selectedPermissions);
        _creating = false;

        if (result.IsFailure)
        {
            _createError = result.Error?.Message ?? "Failed to create role.";
            return;
        }

        _createSuccess = $"Role \"{_roleName.Trim()}\" created successfully.";
        Snackbar.Add(_createSuccess, Severity.Success);
        _roleName = string.Empty;
        _selectedPermissions.Clear();
    }
}
