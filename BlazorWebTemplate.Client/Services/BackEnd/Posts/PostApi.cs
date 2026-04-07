using System.Net.Http.Json;
using BlazorWebTemplate.Client.Services.BackEnd.Infrastructure.Http;
using BlazorWebTemplate.Shared.Common;
using BlazorWebTemplate.Shared.Posts;
using Microsoft.Extensions.Logging;

namespace BlazorWebTemplate.Client.Services.BackEnd.Posts;

internal sealed class PostApi(
    IHttpClientFactory httpClientFactory,
    ILogger<PostApi> logger) : BaseApiService(logger), IPostApi
{
    public async Task<ApiResult<DummyJsonPostListResponseDto>> GetPostsAsync(PostQuery query, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(DependencyInjection.ApiClientName);
        var path = string.IsNullOrWhiteSpace(query.Search)
            ? $"posts?limit={query.NormalizedPageSize}&skip={query.Skip}"
            : $"posts/search?q={Uri.EscapeDataString(query.Search)}&limit={query.NormalizedPageSize}&skip={query.Skip}";

        using var request = new HttpRequestMessage(HttpMethod.Get, path);
        return await SendForResultAsync<DummyJsonPostListResponseDto>(client, request, cancellationToken);
    }

    public async Task<ApiResult<DummyJsonPostDto>> GetPostAsync(int id, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(DependencyInjection.ApiClientName);
        using var request = new HttpRequestMessage(HttpMethod.Get, $"posts/{id}");
        return await SendForResultAsync<DummyJsonPostDto>(client, request, cancellationToken);
    }

    public async Task<ApiResult<DummyJsonPostDto>> CreateAsync(DummyJsonPostWriteDto dto, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(DependencyInjection.ApiClientName);
        using var request = new HttpRequestMessage(HttpMethod.Post, "posts/add")
        {
            Content = JsonContent.Create(dto),
        };

        return await SendForResultAsync<DummyJsonPostDto>(client, request, cancellationToken);
    }

    public async Task<ApiResult<DummyJsonPostDto>> UpdateAsync(int id, DummyJsonPostWriteDto dto, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(DependencyInjection.ApiClientName);
        using var request = new HttpRequestMessage(HttpMethod.Put, $"posts/{id}")
        {
            Content = JsonContent.Create(dto),
        };

        return await SendForResultAsync<DummyJsonPostDto>(client, request, cancellationToken);
    }

    public async Task<ApiResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(DependencyInjection.ApiClientName);
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"posts/{id}");
        return await SendAsync(client, request, cancellationToken);
    }
}
