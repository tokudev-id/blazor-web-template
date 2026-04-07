using BlazorWebTemplate.Client.Services.BackEnd.Categories;
using BlazorWebTemplate.Shared.Categories;
using BlazorWebTemplate.Web.Common.Constants;
using BlazorWebTemplate.Web.Services.Shell;
using Microsoft.AspNetCore.Components;

namespace BlazorWebTemplate.Web.Features.Categories.Pages;

public partial class Categories
{
    [Inject]
    private ICategoryService CategoryService { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [SupplyParameterFromQuery(Name = "search")]
    public string? Search { get; set; }

    protected IReadOnlyList<AppShellBreadcrumb> _breadcrumbs = AppBreadcrumbs.Categories();
    protected List<CategorySummary> _categories = [];
    protected bool _isLoading = true;
    protected string? _errorMessage;
    protected string _searchInput = string.Empty;

    protected override async Task OnParametersSetAsync()
    {
        _isLoading = true;
        _searchInput = Search ?? string.Empty;
        var result = await CategoryService.GetCategoriesAsync(Search);
        _isLoading = false;

        if (result.IsFailure)
        {
            _errorMessage = result.Error?.Message ?? "The categories could not be loaded.";
            _categories = [];
            return;
        }

        _errorMessage = null;
        _categories = result.Value?.ToList() ?? [];
    }

    protected void ApplySearch()
        => NavigationManager.NavigateTo(AppRoutes.CategoriesSearch(_searchInput), forceLoad: true);

    protected void ClearSearch()
        => NavigationManager.NavigateTo(AppRoutes.Categories, forceLoad: true);
}
