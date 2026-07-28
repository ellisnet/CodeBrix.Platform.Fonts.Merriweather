using System;
using System.IO;

namespace CodeBrix.Platform.Fonts.Merriweather.Tests;

internal static class TestAssetPaths
{
    public static string TestAssetsRoot { get; } =
        Path.Combine(AppContext.BaseDirectory, "TestAssets");

    public static string FontsFolder { get; } =
        Path.Combine(TestAssetsRoot, "Fonts");

    public static string ManifestPath { get; } =
        Path.Combine(FontsFolder, "Merriweather.ttf.manifest");

    public static string VariableFontPath { get; } =
        Path.Combine(FontsFolder, "Merriweather.ttf");

    public static string UprimarkerPath { get; } =
        Path.Combine(TestAssetsRoot, "CodeBrix.Platform.Fonts.Merriweather.uprimarker");

    public static string TargetsFilePath { get; } =
        Path.Combine(TestAssetsRoot, "buildTransitive", "net10.0", "CodeBrix.Platform.Fonts.Merriweather.OflLicenseForever.targets");

    public static string DescriptorPath { get; } =
        Path.Combine(TestAssetsRoot, "CODEBRIX-DEVELOP.json");

    /// <summary>
    /// The companion families that supply the scripts Merriweather itself does not
    /// carry: Greek (Noto Serif), Armenian and Georgian. Each ships a variable font
    /// plus its own manifest.
    /// </summary>
    public static string[] CompanionFamilies { get; } =
        ["NotoSerif", "NotoSerifArmenian", "NotoSerifGeorgian"];

    public static string CompanionFontPath(string family) =>
        Path.Combine(FontsFolder, family + ".ttf");

    public static string CompanionManifestPath(string family) =>
        Path.Combine(FontsFolder, family + ".ttf.manifest");
}
