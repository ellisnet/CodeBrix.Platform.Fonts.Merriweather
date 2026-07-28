using System.IO;
using System.Linq;
using SilverAssertions;
using Xunit;

namespace CodeBrix.Platform.Fonts.Merriweather.Tests;

public class ContentFilePresenceTests
{
    [Fact]
    public void Variable_font_Merriweather_ttf_is_present()
        => File.Exists(TestAssetPaths.VariableFontPath).Should().BeTrue();

    [Fact]
    public void Manifest_file_is_present()
        => File.Exists(TestAssetPaths.ManifestPath).Should().BeTrue();

    [Fact]
    public void Total_ttf_count_is_52()
    {
        //Arrange/Act
        // 1 Merriweather variable + 24 Merriweather statics, then the three
        // companion families: Noto Serif (1 + 12), Noto Serif Armenian (1 + 6)
        // and Noto Serif Georgian (1 + 6).
        var ttfFiles = Directory.GetFiles(TestAssetPaths.FontsFolder, "*.ttf");

        //Assert
        ttfFiles.Length.Should().Be(52);
    }

    [Fact]
    public void All_24_static_Merriweather_fonts_are_present()
    {
        //Arrange
        // Note the static font naming convention shared across these packages:
        // the italic of the Regular weight is just "Italic" (no "Regular"
        // prefix), e.g. Merriweather-Italic.ttf. Every other weight carries its
        // weight name in the italic filename. Merriweather publishes no
        // Condensed stretch, so only Normal and SemiCondensed ship.
        var weights = new[] { "Light", "Regular", "Medium", "SemiBold", "Bold", "ExtraBold" };
        var styles = new[] { "", "Italic" };
        var stretches = new[] { "", "_SemiCondensed" };

        //Act
        var missing = (
            from weight in weights
            from style in styles
            from stretch in stretches
            let weightSegment = (weight == "Regular" && style == "Italic") ? "" : weight
            let fileName = $"Merriweather{stretch}-{weightSegment}{style}.ttf"
            let path = Path.Combine(TestAssetPaths.FontsFolder, fileName)
            where !File.Exists(path)
            select fileName
        ).ToList();

        //Assert
        missing.Should().BeEmpty();
    }

    [Theory]
    [InlineData("NotoSerif")]
    [InlineData("NotoSerifArmenian")]
    [InlineData("NotoSerifGeorgian")]
    public void Companion_variable_font_is_present(string family)
        => File.Exists(TestAssetPaths.CompanionFontPath(family)).Should().BeTrue();

    [Theory]
    [InlineData("NotoSerif")]
    [InlineData("NotoSerifArmenian")]
    [InlineData("NotoSerifGeorgian")]
    public void Companion_manifest_is_present(string family)
        => File.Exists(TestAssetPaths.CompanionManifestPath(family)).Should().BeTrue();

    [Fact]
    public void All_12_static_NotoSerif_fonts_are_present()
    {
        //Arrange — Noto Serif supplies Greek, and ships upright plus italic.
        var weights = new[] { "Light", "Regular", "Medium", "SemiBold", "Bold", "ExtraBold" };
        var styles = new[] { "", "Italic" };

        //Act
        var missing = (
            from weight in weights
            from style in styles
            let weightSegment = (weight == "Regular" && style == "Italic") ? "" : weight
            let fileName = $"NotoSerif-{weightSegment}{style}.ttf"
            where !File.Exists(Path.Combine(TestAssetPaths.FontsFolder, fileName))
            select fileName
        ).ToList();

        //Assert
        missing.Should().BeEmpty();
    }

    [Theory]
    [InlineData("NotoSerifArmenian")]
    [InlineData("NotoSerifGeorgian")]
    public void All_6_static_fonts_are_present_for(string family)
    {
        //Arrange — neither family has an italic face upstream, so only the six
        //upright weights ship.
        var weights = new[] { "Light", "Regular", "Medium", "SemiBold", "Bold", "ExtraBold" };

        //Act
        var missing = weights
            .Select(weight => $"{family}-{weight}.ttf")
            .Where(fileName => !File.Exists(Path.Combine(TestAssetPaths.FontsFolder, fileName)))
            .ToList();

        //Assert
        missing.Should().BeEmpty();
    }

    [Fact]
    public void No_optical_size_token_survives_in_any_font_name()
    {
        //Arrange — only the 24pt optical size ships (the manifest schema has no
        //opsz field), so the upstream "_24pt" token is stripped on the way in.
        var offenders = Directory.GetFiles(TestAssetPaths.FontsFolder, "*.ttf")
            .Select(Path.GetFileName)
            .Where(name => name!.Contains("pt-") || name.Contains("pt_"))
            .ToList();

        //Assert
        offenders.Should().BeEmpty();
    }

    [Fact]
    public void Uprimarker_file_is_present()
        => File.Exists(TestAssetPaths.UprimarkerPath).Should().BeTrue();

    [Fact]
    public void Uprimarker_file_is_empty()
    {
        //Arrange
        var info = new FileInfo(TestAssetPaths.UprimarkerPath);

        //Assert
        info.Length.Should().Be(0L);
    }

    [Fact]
    public void Variable_font_is_non_trivial_size()
    {
        //Arrange
        var info = new FileInfo(TestAssetPaths.VariableFontPath);

        //Assert
        info.Length.Should().BeGreaterThan(100_000L);
    }
}
