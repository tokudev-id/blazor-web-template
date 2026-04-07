using BlazorWebTemplate.Client.Services.BackEnd.Posts;
using BlazorWebTemplate.Shared.Common;
using BlazorWebTemplate.Shared.Posts;
using BlazorWebTemplate.Web.Common.Constants;
using BlazorWebTemplate.Web.Features.Posts.State;
using BlazorWebTemplate.Web.Services.Shell;
using Microsoft.AspNetCore.Components;
using PostSortDirection = BlazorWebTemplate.Shared.Common.SortDirection;

namespace BlazorWebTemplate.Web.Features.Posts.Pages;

public partial class Posts
{
    [Inject]
    private IPostService PostService { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    protected PostsPageState PageState { get; set; } = default!;

    [SupplyParameterFromQuery(Name = "page")]
    public int? Page { get; set; }

    [SupplyParameterFromQuery(Name = "q")]
    public string? Query { get; set; }

    [SupplyParameterFromQuery(Name = "tag")]
    public string? Tag { get; set; }

    [SupplyParameterFromQuery(Name = "sort")]
    public string? Sort { get; set; }

    [SupplyParameterFromQuery(Name = "direction")]
    public string? Direction { get; set; }

    [SupplyParameterFromQuery(Name = "size")]
    public int? Size { get; set; }

    protected IReadOnlyList<AppShellBreadcrumb> _breadcrumbs = AppBreadcrumbs.Posts();
    protected PagedResult<PostSummary>? _posts;
    protected bool _isLoading = true;
    protected string? _errorMessage;

    protected override async Task OnParametersSetAsync()
    {
        _isLoading = true;
        _errorMessage = null;

        var request = PageState.ApplyRouteParameters(Page, Query, Tag, Sort, Direction, Size);
        var result = await PostService.GetPostsAsync(request);

        _isLoading = false;

        if (result.IsFailure)
        {
            _errorMessage = result.Error?.Message ?? "The post listing could not be loaded.";
            _posts = null;
            return;
        }

        _posts = result.Value;
    }

    protected void ApplyFilters()
        => NavigationManager.NavigateTo(PageState.BuildApplyLink(), forceLoad: true);

    protected void ClearFilters()
        => NavigationManager.NavigateTo(AppRoutes.Posts, forceLoad: true);
}
