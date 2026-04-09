namespace BlazorWebTemplate.Shared.Common.Responses;

public sealed record ApiError(
    string Code,
    string Message,
    int? StatusCode = null,
    IReadOnlyDictionary<string, string[]>? ValidationErrors = null);
