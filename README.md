# CodeBrix.Platform.Fonts.Merriweather

A redistribution of the Merriweather font family packaged as a CodeBrix-family NuGet library for .NET 10 applications.
CodeBrix.Platform.Fonts.Merriweather is a content-files font package for CodeBrix.Platform-forked applications — supplying the Merriweather variable font and its static instances as build-time assets — and is equally usable as a plain content-files NuGet in any .NET 10 project that wants the Merriweather font set.
Merriweather covers the Latin and Cyrillic scripts but not Greek, Armenian or Georgian, so this package also bundles three Noto Serif companion families that supply those scripts in a matching serif design.
The library has no managed dependencies other than .NET, and is provided as a .NET 10 library and associated `CodeBrix.Platform.Fonts.Merriweather.OflLicenseForever` NuGet package.

CodeBrix.Platform.Fonts.Merriweather supports applications and assemblies that target Microsoft .NET version 10.0 and later.
Microsoft .NET version 10.0 is a Long-Term Supported (LTS) version of .NET, and was released on Nov 11, 2025; and will be actively supported by Microsoft until Nov 14, 2028.
Please update your C#/.NET code and projects to the latest LTS version of Microsoft .NET.

## CodeBrix.Platform.Fonts.Merriweather supports:

* The Merriweather variable font (`Merriweather.ttf`) covering the full weight axis (300-900), width axis and optical-size axis, used directly on every platform.
* 24 static `.ttf` font files covering the Light/Regular/Medium/SemiBold/Bold/ExtraBold weights in Normal and Italic styles across the Normal and SemiCondensed stretches — for platforms that resolve fonts through the static-instance manifest.
* Three companion font families that extend script coverage beyond what Merriweather itself carries:
  * **Noto Serif** (`NotoSerif.ttf` plus 12 static instances) — the Greek script, upright and italic.
  * **Noto Serif Armenian** (`NotoSerifArmenian.ttf` plus 6 static instances) — the Armenian script.
  * **Noto Serif Georgian** (`NotoSerifGeorgian.ttf` plus 6 static instances) — the Georgian script.
* A `.ttf.manifest` JSON file per family that maps `font_style` / `font_weight` / `font_stretch` triples to the matching static font file.
* A `CODEBRIX-DEVELOP.json` descriptor that tells CodeBrix.Develop how to wire this font into a generated application and which software-keyboard layouts the package's glyph coverage supports.
* A `buildTransitive` MSBuild `.targets` file (hooking into the CodeBrix.Platform `_CodeBrixAddLibraryAssets` target) that prunes the redundant static font files at build time on platforms that don't need them, while always keeping the four variable fonts available.
* The CodeBrix `.uprimarker` file so CodeBrix.Platform build pipelines discover the package as a UPRI-bearing font asset library.

## Sample Code

### Reference the font from XAML (CodeBrix.Platform app)

```xml
<TextBlock Text="Hello, world."
           FontFamily="ms-appx:///CodeBrix.Platform.Fonts.Merriweather/Fonts/Merriweather.ttf" />
```

### Reference a specific static weight

```xml
<TextBlock Text="Bold sample"
           FontFamily="ms-appx:///CodeBrix.Platform.Fonts.Merriweather/Fonts/Merriweather-Bold.ttf" />
```

### Set Merriweather as the default text font (CodeBrix.Platform app)

```csharp
global::CodeBrix.Platform.UI.FeatureConfiguration.Font.DefaultTextFontFamily =
    "ms-appx:///CodeBrix.Platform.Fonts.Merriweather/Fonts/Merriweather.ttf";
```

Note that the font URI carries no `#FamilyName` fragment. CodeBrix.Platform strips such a fragment before resolving the font, and leaving it on the value assigned to `DefaultTextFontFamily` prevents the startup font-manifest preload from finding the manifest.

## Optical sizes

Merriweather is published upstream in five optical sizes (24, 36, 48, 96 and 120 pt). The CodeBrix font-manifest schema addresses fonts by style, weight and stretch only and has no optical-size dimension, so a single optical size is shipped as static instances: **24pt**, chosen as the closest static to the variable font's own `opsz` default of 18. The bundled variable font retains the full optical-size axis.

## License

The entire package — the library code, the `.targets` file, the packaging wrapper, and the bundled Merriweather and Noto Serif `.ttf` font files — is licensed under the SIL Open Font License, Version 1.1. see: https://en.wikipedia.org/wiki/SIL_Open_Font_License

The full license text is bundled with this repository as `OFL.txt` at the repository root and is also packaged inside the produced NuGet under the same name. The package is published under the SPDX expression `OFL-1.1`.

Merriweather is distributed with the Reserved Font Name "Merriweather"; the bundled font files are redistributed bit-for-bit unmodified and their internal name tables are untouched. See `THIRD-PARTY-NOTICES.txt` for the full attribution of all four bundled font families.
