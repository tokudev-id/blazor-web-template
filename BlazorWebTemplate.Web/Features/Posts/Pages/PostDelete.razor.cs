using BlazorWebTemplate.Client.Services.BackEnd.Posts;
using BlazorWebTemplate.Shared.Posts;
using BlazorWebTemplate.Web.Common.Constants;
using BlazorWebTemplate.Web.Services.Shell;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace BlazorWebTemplate.Web.Features.Posts.Pages;

public partial class PostDelete
{
    [Inject]
    private IPostService PostService { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private AppShellState ShellState { get; set; } = default!;

    [Parameter]
    public int Id { get; set; }

    protected IReadOnlyList<AppShellBreadcrumb> _breadcrumbs = AppBreadcrumbs.PostDelete();
    protected PostDetail? _post;
    protected string? _errorMessage;
    protected bool _isLoading = true;
    protected bool _isSubmitting;
    protected readonly object _deleteFormModel = new();
    protected string _confirmationInput = string.Empty;

    protected bool CanDelete => _post is not null && string.Equals(_confirmationInput.Trim(), _post.Title, StringComparison.Ordinal);

    protected override async Task OnParametersSetAsync()
    {
        _isLoading = true;
        var result = await PostService.GetPostAsync(Id);
        _isLoading = false;

        if (result.IsFailure)
        {
            _errorMessage = result.Error?.Message ?? "The post could not be loaded.";
            return;
        }

        _post = result.Value;
    }

    protected async Task HandleDeleteAsync(EditContext _)
    {
        if (_isSubmitting)
        {
            return;
        }

        _isSubmitting = true;
        var result = await PostService.DeleteAsync(Id);
        _isSubmitting = false;

        if (result.IsFailure)
        {
            _errorMessage = result.Error?.Message ?? "The post could not be deleted.";
            return;
        }

        await ShellState.NotifyAsync(new AppShellNotification(Severity.Success, "Post deleted."));
        NavigationManager.NavigateTo(AppRoutes.Posts, forceLoad: true);
    }
}
