using System.IO;
using SilverAssertions;
using Xunit;

namespace CodeBrix.Platform.Fonts.Merriweather.Tests;

public class TargetsFileTests
{
    [Fact]
    public void Targets_file_is_present()
        => File.Exists(TestAssetPaths.TargetsFilePath).Should().BeTrue();

    [Fact]
    public void Targets_file_declares_codebrix_target_name()
    {
        //Arrange
        var content = File.ReadAllText(TestAssetPaths.TargetsFilePath);

        //Assert
        content.Should().Contain("Name=\"CodeBrixRemoveUnusedMerriweather\"");
    }

    [Fact]
    public void Targets_file_hooks_after_codebrix_add_library_assets()
    {
        //Arrange
        var content = File.ReadAllText(TestAssetPaths.TargetsFilePath);

        //Assert
        content.Should().Contain("AfterTargets=\"_CodeBrixAddLibraryAssets\"");
    }

    [Fact]
    public void Targets_file_uses_net10_lib_paths()
    {
        //Arrange
        var content = File.ReadAllText(TestAssetPaths.TargetsFilePath);

        //Assert
        content.Should().Contain("lib\\net10.0\\CodeBrix.Platform.Fonts.Merriweather\\Fonts");
    }

    [Fact]
    public void Targets_file_contains_no_foreign_family_token()
    {
        //Arrange
        var content = File.ReadAllText(TestAssetPaths.TargetsFilePath);

        //Assert
        content.Should().NotContain("Roboto");
    }

    [Fact]
    public void Targets_file_supports_font_manifest_condition_present()
    {
        //Arrange
        var content = File.ReadAllText(TestAssetPaths.TargetsFilePath);

        //Assert
        content.Should().Contain("$(SupportsFontManifest)");
    }

    [Fact]
    public void Targets_file_never_removes_a_variable_font()
    {
        //Arrange
        var content = File.ReadAllText(TestAssetPaths.TargetsFilePath);

        //Assert
        // The variable fonts (no dash in the file name) must not appear in a
        // Remove= expression; only the dash-bearing static fonts are pruned.
        // The three companions matter most here: they carry the Greek, Armenian
        // and Georgian scripts, so pruning them would silently drop coverage.
        content.Should().NotContain("Fonts\\Merriweather.ttf\"");
        content.Should().NotContain("Fonts\\NotoSerif.ttf\"");
        content.Should().NotContain("Fonts\\NotoSerifArmenian.ttf\"");
        content.Should().NotContain("Fonts\\NotoSerifGeorgian.ttf\"");
    }
}
