using System.Text.RegularExpressions;

namespace BlazorWebTemplate.Tests.Architecture;

public sealed class ProjectStructureTests
{
    private static readonly string RepoRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../"));

    [Fact]
    public void WebProject_ReferencesOnlyClientAndShared()
    {
        var projectFile = ReadProjectFile("BlazorWebTemplate.Web/BlazorWebTemplate.Web.csproj");

        Assert.Contains("BlazorWebTemplate.Client", projectFile);
        Assert.Contains("BlazorWebTemplate.Shared", projectFile);
        Assert.DoesNotContain("BlazorWebTemplate.Tests", projectFile);
    }

    [Fact]
    public void ClientProject_ReferencesOnlyShared()
    {
        var projectFile = ReadProjectFile("BlazorWebTemplate.Client/BlazorWebTemplate.Client.csproj");

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

    [Fact]
    public void WebProject_UsesEnterpriseFolderStructure()
    {
        var webRoot = Path.Combine(RepoRoot, "BlazorWebTemplate.Web");

        Assert.True(Directory.Exists(Path.Combine(webRoot, "Common")));
        Assert.True(Directory.Exists(Path.Combine(webRoot, "Layouts")));
        Assert.True(Directory.Exists(Path.Combine(webRoot, "Services")));
        Assert.True(Directory.Exists(Path.Combine(webRoot, "Features", "Posts", "Pages")));
        Assert.True(Directory.Exists(Path.Combine(webRoot, "Features", "Posts", "State")));
    }

    [Fact]
    public void CommonFolder_DoesNotReferenceFeatureNamespaces()
    {
        var commonRoot = Path.Combine(RepoRoot, "BlazorWebTemplate.Web", "Common");
        var violations = Directory
            .EnumerateFiles(commonRoot, "*.*", SearchOption.AllDirectories)
            .Where(path => path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase) || path.EndsWith(".razor", StringComparison.OrdinalIgnoreCase))
            .Where(path => File.ReadAllText(path).Contains("BlazorWebTemplate.Web.Features.", StringComparison.Ordinal))
            .ToList();

        Assert.True(violations.Count == 0, $"Common should not depend on feature namespaces. Violations: {string.Join(", ", violations.Select(Path.GetFileName))}");
    }

    [Fact]
    public void Layouts_DoNotDependOnFeatureStateTypes()
    {
        var layoutsRoot = Path.Combine(RepoRoot, "BlazorWebTemplate.Web", "Layouts");
        var violations = Directory
            .EnumerateFiles(layoutsRoot, "*.*", SearchOption.AllDirectories)
            .Where(path => path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase) || path.EndsWith(".razor", StringComparison.OrdinalIgnoreCase))
            .Where(path => Regex.IsMatch(File.ReadAllText(path), @"Features\.[A-Za-z]+\.(Pages|State)"))
            .ToList();

        Assert.True(violations.Count == 0, $"Layouts should not depend on feature page/state types. Violations: {string.Join(", ", violations.Select(Path.GetFileName))}");
    }

    [Fact]
    public void ServicesAggregator_ComposesCapabilityModules()
    {
        var file = ReadProjectFile("BlazorWebTemplate.Web/Services/DependencyInjection.cs");

        Assert.Contains("AddAppInfoServices", file);
        Assert.Contains("AddAuthenticationServices", file);
        Assert.Contains("AddAuthorizationServices", file);
        Assert.Contains("AddFrontEndServices", file);
        Assert.Contains("AddShellServices", file);
        Assert.Contains("AddUiServices", file);
    }

    [Fact]
    public void SharedProject_DoesNotContainProviderSpecificDummyJsonTypes()
    {
        var sharedRoot = Path.Combine(RepoRoot, "BlazorWebTemplate.Shared");
        var violations = Directory
            .EnumerateFiles(sharedRoot, "*.*", SearchOption.AllDirectories)
            .Where(path => path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
            .Where(path => File.ReadAllText(path).Contains("DummyJson", StringComparison.Ordinal))
            .ToList();

        Assert.True(violations.Count == 0, $"Shared should not contain provider-specific DummyJson types. Violations: {string.Join(", ", violations.Select(Path.GetFileName))}");
    }

    [Fact]
    public void ClientFeatureServices_DoNotUseRawHttpClientDirectly()
    {
        var clientRoot = Path.Combine(RepoRoot, "BlazorWebTemplate.Client", "Services", "BackEnd");
        var candidateFiles = Directory
            .EnumerateFiles(clientRoot, "*Service.cs", SearchOption.AllDirectories)
            .Where(path => !path.Contains("/Infrastructure/"));

        var violations = candidateFiles
            .Where(path => Regex.IsMatch(File.ReadAllText(path), @"\bHttpClient\b"))
            .ToList();

        Assert.True(violations.Count == 0, $"Client feature services should not use raw HttpClient directly. Violations: {string.Join(", ", violations.Select(Path.GetFileName))}");
    }

    private static string ReadProjectFile(string relativePath)
        => File.ReadAllText(Path.Combine(RepoRoot, relativePath));
}
