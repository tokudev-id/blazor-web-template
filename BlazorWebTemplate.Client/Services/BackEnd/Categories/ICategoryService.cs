using BlazorWebTemplate.Shared.Categories;
using BlazorWebTemplate.Shared.Common;

namespace BlazorWebTemplate.Client.Services.BackEnd.Categories;

public interface ICategoryService
{
    Task<ApiResult<IReadOnlyList<CategorySummary>>> GetCategoriesAsync(string? search = null, CancellationToken cancellationToken = default);
}
