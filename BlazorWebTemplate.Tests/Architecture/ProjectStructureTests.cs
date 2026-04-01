using System.Text.RegularExpressions;

namespace BlazorWebTemplate.Tests.Architecture;

public sealed class ProjectStructureTests
{
    private static readonly string RepoRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../"));

    [Fact]
    public void WebProject_ReferencesOnlyBackendAndShared()
    {
        var projectFile = ReadProjectFile("BlazorWebTemplate.Web/BlazorWebTemplate.Web.csproj");

        Assert.Contains("BlazorWebTemplate.Backend", projectFile);
        Assert.Contains("BlazorWebTemplate.Shared", projectFile);
        Assert.DoesNotContain("BlazorWebTemplate.Tests", projectFile);
    }

    [Fact]
    public void BackendProject_ReferencesOnlyShared()
    {
        var projectFile = ReadProjectFile("BlazorWebTemplate.Backend/BlazorWebTemplate.Backend.csproj");

        Assert.Contains("BlazorWebTemplate.Shared", projectFile);
        Assert.DoesNotContain("BlazorWebTemplate.Web", projectFile);
        Assert.DoesNotContain("BlazorWebTemplate.Tests", projectFile);
    }

    [Fact]
    public void SharedProject_HasNoProjectReferences()
    {
        var projectFile = ReadProjectFile("BlazorWebTemplate.Shared/BlazorWebTemplate.Shared.csproj");

        Assert.DoesNotContain("ProjectReference", projectFile);
    }

    [Fact]
    public void WebProject_DoesNotUseRawHttpClientInComponentsOrFeatures()
    {
        var webRoot = Path.Combine(RepoRoot, "BlazorWebTemplate.Web");
        var candidateFiles = Directory
            .EnumerateFiles(webRoot, "*.*", SearchOption.AllDirectories)
            .Where(path => path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase) || path.EndsWith(".razor", StringComparison.OrdinalIgnoreCase))
            .Where(path => !path.Contains("/bin/") && !path.Contains("/obj/"))
            .Where(path => !path.EndsWith("Program.cs", StringComparison.OrdinalIgnoreCase));

        var violations = candidateFiles
            .Where(path => Regex.IsMatch(File.ReadAllText(path), @"\bHttpClient\b"))
            .ToList();

        Assert.True(violations.Count == 0, $"Web project should not use raw HttpClient directly. Violations: {string.Join(", ", violations.Select(Path.GetFileName))}");
    }

    private static string ReadProjectFile(string relativePath)
        => File.ReadAllText(Path.Combine(RepoRoot, relativePath));
}
