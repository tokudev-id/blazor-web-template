using BlazorWebTemplate.Shared.Categories;
using BlazorWebTemplate.Shared.Common;

namespace BlazorWebTemplate.Client.Services.BackEnd.Categories;

internal sealed class CategoryService(ICategoryApi categoriesApiClient) : ICategoryService
{
    public async Task<ApiResult<IReadOnlyList<CategorySummary>>> GetCategoriesAsync(string? search = null, CancellationToken cancellationToken = default)
    {
        var result = await categoriesApiClient.GetCategoriesAsync(cancellationToken);
        if (result.IsFailure || result.Value is null)
        {
            return ApiResult<IReadOnlyList<CategorySummary>>.Failure(result.Error ?? new ApiError(ApiErrorCodes.Unknown, "Unable to load categories."));
        }

        var items = result.Value
            .Where(category => string.IsNullOrWhiteSpace(search)
                || category.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
                || category.Slug.Contains(search, StringComparison.OrdinalIgnoreCase))
            .Select(category =>
            {
                var title = string.IsNullOrWhiteSpace(category.Name) ? category.Slug : category.Name;
                var slug = string.IsNullOrWhiteSpace(category.Slug) ? title : category.Slug;
                return new CategorySummary(
                    title,
                    slug,
                    $"Use {title} as a starter taxonomy bucket for dashboard content.",
                    $"/posts?tag={Uri.EscapeDataString(slug)}");
            })
            .Cast<CategorySummary>()
            .ToList();

        return ApiResult<IReadOnlyList<CategorySummary>>.Success(items);
    }
}
