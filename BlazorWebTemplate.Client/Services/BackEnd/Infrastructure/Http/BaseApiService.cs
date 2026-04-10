using System.Net;
using System.Text.Json;
using BlazorWebTemplate.Shared.Common.Constants;
using BlazorWebTemplate.Shared.Common.Responses;
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
        var wrapper = await JsonSerializer.DeserializeAsync<ApiResponseWrapper<T>>(responseStream, SerializerOptions, cancellationToken);

        if (wrapper is null)
        {
            return ApiResult<T>.Failure(new ApiError(ApiErrorCodes.RemoteService, "The backend service returned an empty payload."));
        }

        if (!wrapper.Success || wrapper.Data is null)
        {
            var message = wrapper.Message
                ?? (wrapper.Errors?.Count > 0 ? string.Join("; ", wrapper.Errors) : null)
                ?? "The backend service returned an unsuccessful response.";
            return ApiResult<T>.Failure(new ApiError(ApiErrorCodes.RemoteService, message));
        }

        return ApiResult<T>.Success(wrapper.Data);
    }

    private async Task<ApiError> CreateApiErrorAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var body = response.Content is null
            ? null
            : await response.Content.ReadAsStringAsync(cancellationToken);

        var message = ExtractErrorMessage(body)
            ?? response.ReasonPhrase
            ?? "The backend service returned an error.";

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

    // Handles both ApiResponse { message, errors[] } and ASP.NET ProblemDetails { title, errors{} } formats.
    private static string? ExtractErrorMessage(string? body)
    {
        if (string.IsNullOrWhiteSpace(body)) return null;

        try
        {
            using var doc = JsonDocument.Parse(body);
            var root = doc.RootElement;

            // ProblemDetails validation errors: { "errors": { "Field": ["msg1"] } }
            if (root.TryGetProperty("errors", out var errorsEl) &&
                errorsEl.ValueKind == JsonValueKind.Object)
            {
                var messages = errorsEl.EnumerateObject()
                    .SelectMany(f => f.Value.ValueKind == JsonValueKind.Array
                        ? f.Value.EnumerateArray().Select(v => v.GetString()).Where(s => s is not null)
                        : [])
                    .ToList();

                if (messages.Count > 0)
                    return string.Join(" ", messages);
            }

            // ProblemDetails title
            if (root.TryGetProperty("title", out var title) &&
                title.ValueKind == JsonValueKind.String)
                return title.GetString();

            // ApiResponse message
            if (root.TryGetProperty("message", out var msg) &&
                msg.ValueKind == JsonValueKind.String)
                return msg.GetString();
        }
        catch { /* not JSON — fall through */ }

        return body.Length > 220 ? body[..220] : body;
    }

    private sealed class ApiResponseWrapper<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }
    }
}
