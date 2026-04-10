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
    protected List<RoleSummary> _roles = [];
    protected bool _rolesLoading = true;
    protected string? _rolesError;
    protected readonly HashSet<string> _deletingRoleId = [];
    protected const int _maxPermsVisible = 4;

    protected override async Task OnInitializedAsync()
    {
        var result = await RbacService.GetRolesAsync();
        _rolesLoading = false;

        if (result.IsFailure)
            _rolesError = result.Error?.Message ?? "Could not load roles.";
        else
            _roles = result.Value ?? [];
    }

    protected Task OpenCreateRoleDialogAsync()
    {
        // Placeholder — create role dialog will be implemented in the manage view
        Snackbar.Add("Role management will be available in the dedicated manage view.", Severity.Info);
        return Task.CompletedTask;
    }

    protected async Task DeleteRoleAsync(RoleSummary role)
    {
        var parameters = new DialogParameters<ConfirmDialog>
        {
            { x => x.Title, $"Delete \"{role.Name}\"?" },
            { x => x.Message, $"This will permanently remove the \"{role.Name}\" role and all its permission assignments." },
            { x => x.WarningMessage, "Users with this role will lose associated permissions." },
        };

        var options = new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<ConfirmDialog>("Confirm delete", parameters, options);
        var dialogResult = await dialog.Result;

        if (dialogResult is null || dialogResult.Canceled) return;

        _deletingRoleId.Add(role.Id);

        var result = await RbacService.DeleteRoleAsync(role.Id);
        _deletingRoleId.Remove(role.Id);

        if (result.IsFailure)
        {
            Snackbar.Add(result.Error?.Message ?? "Failed to delete role.", Severity.Error);
            return;
        }

        _roles.RemoveAll(r => r.Id == role.Id);
        Snackbar.Add($"Role \"{role.Name}\" deleted.", Severity.Success);
        StateHasChanged();
    }
}
