namespace BlazorWebTemplate.Shared.Categories;

public sealed record CategorySummary(
    string Name,
    string Slug,
    string Description,
    string Url);
