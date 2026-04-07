using BlazorWebTemplate.Client.Services.BackEnd.Posts;
using BlazorWebTemplate.Shared.Common;
using BlazorWebTemplate.Shared.Posts;
using BlazorWebTemplate.Web.Common.Constants;
using BlazorWebTemplate.Web.Services.Shell;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using MudBlazor;

namespace BlazorWebTemplate.Web.Features.Posts.Pages;

public partial class PostEditor
{
    [Inject]
    private IPostService PostService { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    private AppShellState ShellState { get; set; } = default!;

    [Parameter]
    public int? Id { get; set; }

    [SupplyParameterFromForm]
    protected PostUpsertRequest Model { get; set; } = new();

    protected bool IsEditMode => Id.HasValue;
    protected string? _saveErrorMessage;
    protected string? _loadErrorMessage;
    protected bool _isSubmitting;
    protected int? _loadedForPostId;
    protected string _baselineSignature = string.Empty;
    protected bool _allowNavigation;
    protected IReadOnlyList<AppShellBreadcrumb> _breadcrumbs = AppBreadcrumbs.PostEditor(false);

    protected bool IsDirty => !_allowNavigation && BuildSignature(Model) != _baselineSignature;

    protected override async Task OnParametersSetAsync()
    {
        _loadErrorMessage = null;
        _breadcrumbs = AppBreadcrumbs.PostEditor(IsEditMode);

        if (!IsEditMode)
        {
            _loadedForPostId = null;
            _baselineSignature = BuildSignature(Model);
            return;
        }

        if (_loadedForPostId == Id)
        {
            return;
        }

        var result = await PostService.GetPostAsync(Id!.Value);
        if (result.IsFailure || result.Value is null)
        {
            _loadErrorMessage = result.Error?.Message ?? "The existing post could not be loaded.";
            _loadedForPostId = Id;
            return;
        }

        Model = new PostUpsertRequest
        {
            Title = result.Value.Title,
            Body = result.Value.Body,
            Tags = string.Join(", ", result.Value.Tags),
        };

        _loadedForPostId = Id;
        _baselineSignature = BuildSignature(Model);
    }

    protected async Task HandleSubmitAsync()
    {
        if (_isSubmitting)
        {
            return;
        }

        _isSubmitting = true;
        _saveErrorMessage = null;

        ApiResult<PostDetail> result = IsEditMode
            ? await PostService.UpdateAsync(Id!.Value, Model)
            : await PostService.CreateAsync(Model);

        _isSubmitting = false;

        if (result.IsFailure || result.Value is null)
        {
            _saveErrorMessage = result.Error?.Message ?? "The post could not be saved.";
            return;
        }

        _allowNavigation = true;
        await ShellState.NotifyAsync(new AppShellNotification(Severity.Success, IsEditMode ? "Post changes saved." : "Post created successfully."));
        NavigationManager.NavigateTo(AppRoutes.PostDetails(result.Value.Id), forceLoad: true);
    }

    protected async ValueTask ConfirmNavigateAsync(LocationChangingContext context)
    {
        if (!IsDirty)
        {
            return;
        }

        var confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "You have unsaved changes. Leave this page?");
        if (!confirmed)
        {
            context.PreventNavigation();
        }
    }

    private static string BuildSignature(PostUpsertRequest request)
        => string.Join("::", request.Title.Trim(), request.Tags.Trim(), request.Body.Trim());
}
