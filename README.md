# CodeBrix.Platform.Fonts.Merriweather

A redistribution of the Merriweather font family packaged as a CodeBrix-family NuGet library for .NET 10 applications.
CodeBrix.Platform.Fonts.Merriweather is a content-files font package for CodeBrix.Platform applications — supplying the Merriweather variable font and its static instances as build-time assets — and is equally usable as a plain content-files NuGet in any .NET 10 project that wants the Merriweather font set.
Merriweather covers the Latin and Cyrillic scripts but not Greek, Armenian or Georgian, so this package also bundles three Noto Serif companion families that supply those scripts in a matching serif design.
The library has no managed dependencies other than .NET, and is provided as a .NET 10 library and associated `CodeBrix.Platform.Fonts.Merriweather.OflLicenseForever` NuGet package.

CodeBrix.Platform.Fonts.Merriweather supports applications and assemblies that target Microsoft .NET version 10.0 and later.
Microsoft .NET version 10.0 is a Long-Term Supported (LTS) version of .NET, and was released on Nov 11, 2025; and will be actively supported by Microsoft until Nov 14, 2028.
Please update your C#/.NET code and projects to the latest LTS version of Microsoft .NET.

## Installation

```
dotnet add package CodeBrix.Platform.Fonts.Merriweather.OflLicenseForever
```

Note that the NuGet package ID and the assembly name are different - there is no package named plain `CodeBrix.Platform.Fonts.Merriweather`:

* NuGet package ID: `CodeBrix.Platform.Fonts.Merriweather.OflLicenseForever`
* Assembly and content-folder name: `CodeBrix.Platform.Fonts.Merriweather` - the name that the `ms-appx:///CodeBrix.Platform.Fonts.Merriweather/Fonts/...` URIs shown below resolve against.

The assembly carries no managed API and nothing to `using` - everything a consumer uses is a font file path or an MSBuild property. The package has no dependencies beyond .NET itself.

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

The CodeBrix font-manifest schema addresses fonts by style, weight and stretch only and has no optical-size dimension, so a single optical size is shipped as static instances: **24pt**, the closest available static to the variable font's own `opsz` default of 18. The bundled variable font retains the full optical-size axis.

## Documentation

The NuGet package includes `AGENT-README.txt`, a complete reference and usage guide written for AI coding agents - point your agent at that file when it is writing code or XAML against this package. It covers the full font inventory, the manifest format, weight/style/stretch selection and the script-coverage rules.

Additional sample code and usage examples are available in the `CodeBrix.Platform.Fonts.Merriweather.Tests` project:
https://github.com/ellisnet/CodeBrix.Platform.Fonts.Merriweather/tree/main/tests/CodeBrix.Platform.Fonts.Merriweather.Tests

## License

CodeBrix.Platform.Fonts.Merriweather is licensed under the SIL Open Font License, Version 1.1 - see the
[LICENSE](https://github.com/ellisnet/CodeBrix.Platform.Fonts.Merriweather/blob/main/LICENSE) file. The licence
covers the entire package: the library code, the `.targets` file, the packaging wrapper, and the bundled
Merriweather and Noto Serif `.ttf` font files alike. The same text is bundled at the repository root and inside
the produced NuGet package as `OFL.txt`, and the package is published under the SPDX expression `OFL-1.1`.

Merriweather is distributed with the Reserved Font Name "Merriweather"; the bundled font files are redistributed
bit-for-bit unmodified and their internal name tables are untouched.

For licensing and provenance information about the open source code included in
this package, see [THIRD-PARTY-NOTICES.txt](https://github.com/ellisnet/CodeBrix.Platform.Fonts.Merriweather/blob/main/THIRD-PARTY-NOTICES.txt).
