using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using SilverAssertions;
using Xunit;

namespace CodeBrix.Platform.Fonts.Merriweather.Tests;

public class ContentManifestTests
{
    private const string CodeBrixPathPrefix = "ms-appx:///CodeBrix.Platform.Fonts.Merriweather/Fonts/";

    // This package was authored by mirroring the sibling Roboto package, so the
    // realistic copy-paste regression is a stray "Roboto" token, not an upstream one.
    private const string ForeignFamilyToken = "Roboto";

    private static readonly int[] ExpectedWeights = [300, 400, 500, 600, 700, 800];

    public static TheoryData<string, int> AllManifests => new()
    {
        { "Merriweather", 24 },
        { "NotoSerif", 12 },
        { "NotoSerifArmenian", 6 },
        { "NotoSerifGeorgian", 6 },
    };

    [Fact]
    public void Manifest_file_exists_in_test_output()
        => File.Exists(TestAssetPaths.ManifestPath).Should().BeTrue();

    [Fact]
    public void Manifest_can_be_deserialized()
    {
        //Arrange
        var json = File.ReadAllText(TestAssetPaths.ManifestPath);

        //Act
        var doc = JsonDocument.Parse(json);

        //Assert
        doc.RootElement.TryGetProperty("fonts", out var fonts).Should().BeTrue();
        fonts.ValueKind.Should().Be(JsonValueKind.Array);
    }

    [Theory]
    [MemberData(nameof(AllManifests))]
    public void Manifest_has_the_expected_entry_count(string family, int expected)
    {
        //Arrange
        var entries = ReadManifestEntries(ManifestFor(family));

        //Act/Assert
        entries.Count.Should().Be(expected);
    }

    [Theory]
    [MemberData(nameof(AllManifests))]
    public void Manifest_every_family_name_uses_codebrix_namespace(string family, int expected)
    {
        //Arrange
        _ = expected;
        var entries = ReadManifestEntries(ManifestFor(family));

        //Act
        var nonMatching = entries
            .Where(e => !e.FamilyName.StartsWith(CodeBrixPathPrefix))
            .ToList();

        //Assert
        nonMatching.Should().BeEmpty();
    }

    [Theory]
    [MemberData(nameof(AllManifests))]
    public void Manifest_every_referenced_font_file_exists_on_disk(string family, int expected)
    {
        //Arrange
        _ = expected;
        var entries = ReadManifestEntries(ManifestFor(family));

        //Act
        var missing = entries
            .Select(e => Path.GetFileName(e.FamilyName))
            .Select(name => Path.Combine(TestAssetPaths.FontsFolder, name))
            .Where(path => !File.Exists(path))
            .ToList();

        //Assert
        missing.Should().BeEmpty();
    }

    [Theory]
    [MemberData(nameof(AllManifests))]
    public void Manifest_covers_all_six_weights(string family, int expected)
    {
        //Arrange
        _ = expected;
        var entries = ReadManifestEntries(ManifestFor(family));

        //Act
        var distinctWeights = entries.Select(e => e.FontWeight).Distinct().OrderBy(w => w).ToArray();

        //Assert
        distinctWeights.Should().BeEquivalentTo(ExpectedWeights);
    }

    [Fact]
    public void Manifest_contains_no_foreign_family_tokens()
    {
        //Arrange
        var json = File.ReadAllText(TestAssetPaths.ManifestPath);

        //Act/Assert
        json.Contains(ForeignFamilyToken).Should().BeFalse();
    }

    [Fact]
    public void Merriweather_manifest_covers_normal_and_italic_styles()
    {
        //Arrange
        var entries = ReadManifestEntries(TestAssetPaths.ManifestPath);

        //Act
        var distinctStyles = entries.Select(e => e.FontStyle).Distinct().OrderBy(s => s).ToArray();

        //Assert
        distinctStyles.Should().BeEquivalentTo(new[] { "Italic", "Normal" });
    }

    [Fact]
    public void Merriweather_manifest_covers_normal_and_semicondensed_stretches()
    {
        //Arrange — Merriweather publishes no Condensed stretch.
        var entries = ReadManifestEntries(TestAssetPaths.ManifestPath);

        //Act
        var distinctStretches = entries.Select(e => e.FontStretch).Distinct().OrderBy(s => s).ToArray();

        //Assert
        distinctStretches.Should().BeEquivalentTo(new[] { "Normal", "SemiCondensed" });
    }

    [Fact]
    public void NotoSerif_manifest_covers_normal_and_italic_styles()
    {
        //Arrange — Noto Serif is the Greek companion and has an italic face.
        var entries = ReadManifestEntries(TestAssetPaths.CompanionManifestPath("NotoSerif"));

        //Act
        var distinctStyles = entries.Select(e => e.FontStyle).Distinct().OrderBy(s => s).ToArray();

        //Assert
        distinctStyles.Should().BeEquivalentTo(new[] { "Italic", "Normal" });
    }

    [Theory]
    [InlineData("NotoSerifArmenian")]
    [InlineData("NotoSerifGeorgian")]
    public void Companion_manifest_is_upright_only(string family)
    {
        //Arrange — neither family has an italic face upstream, so italic text in
        //those scripts renders upright. Asserting it here keeps that a decision
        //rather than an accident.
        var entries = ReadManifestEntries(TestAssetPaths.CompanionManifestPath(family));

        //Act
        var distinctStyles = entries.Select(e => e.FontStyle).Distinct().ToArray();

        //Assert
        distinctStyles.Should().BeEquivalentTo(new[] { "Normal" });
    }

    private static string ManifestFor(string family) =>
        family == "Merriweather" ? TestAssetPaths.ManifestPath : TestAssetPaths.CompanionManifestPath(family);

    private static List<ManifestEntry> ReadManifestEntries(string manifestPath)
    {
        var json = File.ReadAllText(manifestPath);
        using var doc = JsonDocument.Parse(json);
        var fonts = doc.RootElement.GetProperty("fonts");

        var list = new List<ManifestEntry>(fonts.GetArrayLength());
        foreach (var entry in fonts.EnumerateArray())
        {
            list.Add(new ManifestEntry(
                entry.GetProperty("font_style").GetString() ?? string.Empty,
                entry.GetProperty("font_weight").GetInt32(),
                entry.GetProperty("font_stretch").GetString() ?? string.Empty,
                entry.GetProperty("family_name").GetString() ?? string.Empty));
        }
        return list;
    }

    private readonly record struct ManifestEntry(
        string FontStyle,
        int FontWeight,
        string FontStretch,
        string FamilyName);
}
