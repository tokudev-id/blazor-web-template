using BlazorWebTemplate.Client.Services.BackEnd.Posts;
using BlazorWebTemplate.Shared.Posts;
using BlazorWebTemplate.Web.Common.Constants;
using BlazorWebTemplate.Web.Services.Shell;
using Microsoft.AspNetCore.Components;

namespace BlazorWebTemplate.Web.Features.Posts.Pages;

public partial class PostDetails
{
    [Inject]
    private IPostService PostService { get; set; } = default!;

    [Parameter]
    public int Id { get; set; }

    protected PostDetail? _post;
    protected bool _isLoading = true;
    protected string? _errorMessage;
    protected IReadOnlyList<AppShellBreadcrumb> _breadcrumbs = AppBreadcrumbs.Posts();

    protected override async Task OnParametersSetAsync()
    {
        _isLoading = true;
        _errorMessage = null;

        var result = await PostService.GetPostAsync(Id);
        _isLoading = false;

        if (result.IsFailure)
        {
            _errorMessage = result.Error?.Message ?? "The post could not be loaded.";
            _breadcrumbs = AppBreadcrumbs.Posts();
            return;
        }

        _post = result.Value;
        _breadcrumbs = _post is null ? AppBreadcrumbs.Posts() : AppBreadcrumbs.PostDetails(_post.Title, Id);
    }
}
