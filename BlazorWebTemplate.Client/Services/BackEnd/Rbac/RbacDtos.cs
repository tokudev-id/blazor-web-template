using System.Text.Json.Serialization;

namespace BlazorWebTemplate.Client.Services.BackEnd.Rbac;

internal sealed class UnictiveCreateRoleRequestDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("permissions")]
    public List<string> Permissions { get; set; } = [];
}

internal sealed class UnictiveRoleDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("permissions")]
    public List<string> Permissions { get; set; } = [];

    [JsonPropertyName("usersCount")]
    public int UsersCount { get; set; }
}

internal sealed class UnictiveUpdateRoleRequestDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

internal sealed class UnictiveUpdateRolePermissionsRequestDto
{
    [JsonPropertyName("permissions")]
    public List<string> Permissions { get; set; } = [];
}
