using System.Net;
using System.Text.Json;
using BlazorWebTemplate.Shared.Common;

namespace BlazorWebTemplate.Backend.Http;

public abstract class ApiClientBase
{
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
            return ApiResult<T>.Failure(new ApiError(ApiErrorCodes.Timeout, "The backend service timed out before it returned a response."));
        }
        catch (HttpRequestException exception)
        {
            return ApiResult<T>.Failure(new ApiError(ApiErrorCodes.RemoteService, $"The backend service could not be reached. {exception.Message}"));
        }
        catch (JsonException)
        {
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
            return ApiResult.Failure(new ApiError(ApiErrorCodes.Timeout, "The backend service timed out before it returned a response."));
        }
        catch (HttpRequestException exception)
        {
            return ApiResult.Failure(new ApiError(ApiErrorCodes.RemoteService, $"The backend service could not be reached. {exception.Message}"));
        }
    }

    private static async Task<ApiResult<T>> ReadResponseAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
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

    private static async Task<ApiError> CreateApiErrorAsync(HttpResponseMessage response, CancellationToken cancellationToken)
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

        return new ApiError(code, message, (int)response.StatusCode);
    }
}
