using BlazorWebTemplate.Shared.Categories;
using BlazorWebTemplate.Shared.Common;

namespace BlazorWebTemplate.Backend.Categories;

public interface ICategoryService
{
    Task<ApiResult<IReadOnlyList<CategorySummary>>> GetCategoriesAsync(string? search = null, CancellationToken cancellationToken = default);
}
