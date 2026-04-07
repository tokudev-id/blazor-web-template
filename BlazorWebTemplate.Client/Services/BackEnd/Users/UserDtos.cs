namespace BlazorWebTemplate.Client.Services.BackEnd.Users;

internal sealed class DummyJsonUserListResponseDto
{
    public List<DummyJsonUserDto> Users { get; set; } = [];
    public int Total { get; set; }
    public int Skip { get; set; }
    public int Limit { get; set; }
}

internal sealed class DummyJsonUserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Role { get; set; }
    public string? Image { get; set; }
    public DummyJsonCompanyDto? Company { get; set; }
}

internal sealed class DummyJsonCompanyDto
{
    public string Name { get; set; } = string.Empty;
}
