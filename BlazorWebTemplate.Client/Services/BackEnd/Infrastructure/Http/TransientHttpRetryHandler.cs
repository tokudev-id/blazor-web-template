using BlazorWebTemplate.Client.Services.BackEnd;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BlazorWebTemplate.Client.Services.BackEnd.Infrastructure.Http;

internal sealed class TransientHttpRetryHandler(
    IOptions<BackEndOptions> optionsAccessor,
    ILogger<TransientHttpRetryHandler> logger) : DelegatingHandler
{
    private static readonly HashSet<HttpMethod> RetryableMethods = [HttpMethod.Get, HttpMethod.Head];

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!RetryableMethods.Contains(request.Method))
        {
            return await base.SendAsync(request, cancellationToken);
        }

        var options = optionsAccessor.Value;
        var attempt = 0;

        while (true)
        {
            var clonedRequest = attempt == 0 ? await request.CloneAsync(cancellationToken) : await request.CloneAsync(cancellationToken);

            try
            {
                var response = await base.SendAsync(clonedRequest, cancellationToken);
                if (!ShouldRetry(response.StatusCode) || attempt >= options.RetryCount)
                {
                    return response;
                }

                logger.LogWarning(
                    "Retrying outbound request {Method} {RequestUri} after {StatusCode} (attempt {Attempt} of {RetryCount}).",
                    request.Method,
                    request.RequestUri,
                    (int)response.StatusCode,
                    attempt + 1,
                    options.RetryCount);

                response.Dispose();
            }
            catch (HttpRequestException exception) when (attempt < options.RetryCount)
            {
                logger.LogWarning(
                    exception,
                    "Retrying outbound request {Method} {RequestUri} after transport failure (attempt {Attempt} of {RetryCount}).",
                    request.Method,
                    request.RequestUri,
                    attempt + 1,
                    options.RetryCount);
            }
            catch (TaskCanceledException exception) when (!cancellationToken.IsCancellationRequested && attempt < options.RetryCount)
            {
                logger.LogWarning(
                    exception,
                    "Retrying outbound request {Method} {RequestUri} after timeout (attempt {Attempt} of {RetryCount}).",
                    request.Method,
                    request.RequestUri,
                    attempt + 1,
                    options.RetryCount);
            }

            attempt++;
            await Task.Delay(options.RetryDelayMilliseconds * attempt, cancellationToken);
        }
    }

    private static bool ShouldRetry(System.Net.HttpStatusCode statusCode)
        => (int)statusCode >= 500 || statusCode == System.Net.HttpStatusCode.TooManyRequests;
}
