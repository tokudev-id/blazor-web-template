using BlazorWebTemplate.Client.Services.BackEnd.Infrastructure.Http;
using BlazorWebTemplate.Shared.Common;
using Microsoft.Extensions.Logging;

namespace BlazorWebTemplate.Client.Services.BackEnd.Categories;

internal sealed class CategoryApi(
    IHttpClientFactory httpClientFactory,
    ILogger<CategoryApi> logger) : BaseApiService(logger), ICategoryApi
{
    public async Task<ApiResult<List<DummyJsonCategoryDto>>> GetCategoriesAsync(CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(DependencyInjection.ApiClientName);
        using var request = new HttpRequestMessage(HttpMethod.Get, "products/categories");
        return await SendForResultAsync<List<DummyJsonCategoryDto>>(client, request, cancellationToken);
    }
}
