using BlazorWebTemplate.Shared.Common;

namespace BlazorWebTemplate.Client.Services.BackEnd.Categories;

internal interface ICategoryApi
{
    Task<ApiResult<List<DummyJsonCategoryDto>>> GetCategoriesAsync(CancellationToken cancellationToken);
}
