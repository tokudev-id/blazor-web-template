namespace BlazorWebTemplate.Shared.Common;

public sealed record ApiError(
    string Code,
    string Message,
    int? StatusCode = null,
    IReadOnlyDictionary<string, string[]>? ValidationErrors = null);
