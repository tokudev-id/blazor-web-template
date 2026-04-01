using BlazorWebTemplate.Backend.Extensions;
using BlazorWebTemplate.Backend.Http;
using BlazorWebTemplate.Shared.Common;

namespace BlazorWebTemplate.Backend.Categories;

internal sealed class DummyJsonCategoriesApiClient(IHttpClientFactory httpClientFactory) : ApiClientBase
{
    public async Task<ApiResult<List<DummyJsonCategoryDto>>> GetCategoriesAsync(CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(ServiceRegistration.ApiClientName);
        using var request = new HttpRequestMessage(HttpMethod.Get, "products/categories");
        return await SendForResultAsync<List<DummyJsonCategoryDto>>(client, request, cancellationToken);
    }
}
