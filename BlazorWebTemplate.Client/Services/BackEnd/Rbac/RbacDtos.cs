using System.Text.Json.Serialization;

namespace BlazorWebTemplate.Client.Services.BackEnd.Rbac;

internal sealed class UnictiveCreateRoleRequestDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("permissions")]
    public List<string> Permissions { get; set; } = [];
}
