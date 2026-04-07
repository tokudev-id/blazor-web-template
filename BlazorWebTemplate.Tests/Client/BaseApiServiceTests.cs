using System.Net;
using BlazorWebTemplate.Client.Services.BackEnd.Infrastructure.Http;
using BlazorWebTemplate.Shared.Common;
using Microsoft.Extensions.Logging;

namespace BlazorWebTemplate.Tests.Client;

public sealed class BaseApiServiceTests
{
    [Fact]
    public async Task SendForResultAsync_MapsNotFoundToApiError()
    {
        using var httpClient = new HttpClient(new StubHandler(new HttpResponseMessage(HttpStatusCode.NotFound)
        {
            Content = new StringContent("missing")
        }))
        {
            BaseAddress = new Uri("https://example.test/")
        };

        var service = new ProbeApiService(LoggerFactory.Create(builder => { }).CreateLogger<ProbeApiService>());
        using var request = new HttpRequestMessage(HttpMethod.Get, "/posts/42");

        var result = await service.SendAsync<object>(httpClient, request, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(ApiErrorCodes.NotFound, result.Error?.Code);
    }

    private sealed class ProbeApiService(ILogger logger) : BaseApiService(logger)
    {
        public Task<ApiResult<T>> SendAsync<T>(HttpClient client, HttpRequestMessage request, CancellationToken cancellationToken)
            => SendForResultAsync<T>(client, request, cancellationToken);
    }

    private sealed class StubHandler(HttpResponseMessage response) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            response.RequestMessage = request;
            return Task.FromResult(response);
        }
    }
}
