using System.Net;
using System.Text.Json;
using BlazorWebTemplate.Shared.Common;
using Microsoft.Extensions.Logging;

namespace BlazorWebTemplate.Client.Services.BackEnd.Infrastructure.Http;

public abstract class BaseApiService
{
    private readonly ILogger _logger;

    protected BaseApiService(ILogger logger)
    {
        _logger = logger;
    }

    protected static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true,
    };

    protected async Task<ApiResult<T>> SendForResultAsync<T>(HttpClient client, HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            return await ReadResponseAsync<T>(response, cancellationToken);
        }
        catch (TaskCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            _logger.LogWarning("Timeout while calling remote service {RequestUri}.", request.RequestUri);
            return ApiResult<T>.Failure(new ApiError(ApiErrorCodes.Timeout, "The backend service timed out before it returned a response."));
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Transport failure while calling remote service {RequestUri}.", request.RequestUri);
            return ApiResult<T>.Failure(new ApiError(ApiErrorCodes.RemoteService, $"The backend service could not be reached. {exception.Message}"));
        }
        catch (JsonException)
        {
            _logger.LogError("Malformed JSON received from remote service {RequestUri}.", request.RequestUri);
            return ApiResult<T>.Failure(new ApiError(ApiErrorCodes.RemoteService, "The backend service returned malformed JSON."));
        }
    }

    protected async Task<ApiResult> SendAsync(HttpClient client, HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return ApiResult.Success();
            }

            return ApiResult.Failure(await CreateApiErrorAsync(response, cancellationToken));
        }
        catch (TaskCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            _logger.LogWarning("Timeout while calling remote service {RequestUri}.", request.RequestUri);
            return ApiResult.Failure(new ApiError(ApiErrorCodes.Timeout, "The backend service timed out before it returned a response."));
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Transport failure while calling remote service {RequestUri}.", request.RequestUri);
            return ApiResult.Failure(new ApiError(ApiErrorCodes.RemoteService, $"The backend service could not be reached. {exception.Message}"));
        }
    }

    private async Task<ApiResult<T>> ReadResponseAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (!response.IsSuccessStatusCode)
        {
            return ApiResult<T>.Failure(await CreateApiErrorAsync(response, cancellationToken));
        }

        if (response.Content.Headers.ContentLength == 0)
        {
            return ApiResult<T>.Failure(new ApiError(ApiErrorCodes.RemoteService, "The backend service returned an empty response."));
        }

        await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var value = await JsonSerializer.DeserializeAsync<T>(responseStream, SerializerOptions, cancellationToken);

        if (value is null)
        {
            return ApiResult<T>.Failure(new ApiError(ApiErrorCodes.RemoteService, "The backend service returned an empty payload."));
        }

        return ApiResult<T>.Success(value);
    }

    private async Task<ApiError> CreateApiErrorAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var body = response.Content is null
            ? null
            : await response.Content.ReadAsStringAsync(cancellationToken);

        var message = string.IsNullOrWhiteSpace(body)
            ? response.ReasonPhrase ?? "The backend service returned an error."
            : body.Length > 220 ? body[..220] : body;

        var code = response.StatusCode switch
        {
            HttpStatusCode.Unauthorized => ApiErrorCodes.Unauthorized,
            HttpStatusCode.Forbidden => ApiErrorCodes.Forbidden,
            HttpStatusCode.NotFound => ApiErrorCodes.NotFound,
            _ => ApiErrorCodes.RemoteService,
        };

        _logger.LogWarning(
            "Remote service returned {StatusCode} for {RequestUri}.",
            (int)response.StatusCode,
            response.RequestMessage?.RequestUri);

        return new ApiError(code, message, (int)response.StatusCode);
    }
}
