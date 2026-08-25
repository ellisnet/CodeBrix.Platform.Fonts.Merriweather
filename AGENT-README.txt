========================================================================
AGENT-README: CodeBrix.Platform.Fonts.Merriweather
A Guide for AI Coding Agents — CONSUMING the
CodeBrix.Platform.Fonts.Merriweather.OflLicenseForever NuGet package
========================================================================


OVERVIEW
========================================================================

CodeBrix.Platform.Fonts.Merriweather is a redistribution of the
Merriweather font family, packaged as a content-asset NuGet library. It
supplies the Merriweather variable font and a curated set of static
instances as build-time content assets for CodeBrix.Platform
applications, and is equally usable as a plain content-files NuGet in
any .NET 10 project that wants the font binaries.

Target framework: .NET 10 or later.

Merriweather covers the Latin and Cyrillic scripts but NOT Greek,
Armenian or Georgian. This package therefore also bundles three Noto
Serif COMPANION families that supply those scripts in a matching serif
design. That is the one structural difference from the sibling
CodeBrix.Platform.Fonts.OpenSans and CodeBrix.Platform.Fonts.Roboto
packages, and it is the thing to understand before writing code against
this package.

The assembly contains no managed code that a consumer calls: it is a
metadata-only .NET 10 DLL whose only purpose is to carry the bundled
font content files. Everything a consumer uses is a file path or an
MSBuild property, not a type. What ships:

  - 52 `.ttf` font files (4 variable + 48 static).
  - Four `.ttf.manifest` JSON files (one per family) mapping
    font_style / font_weight / font_stretch triples to the matching
    static font file.
  - A `CODEBRIX-DEVELOP.json` descriptor at the package root that tells
    CodeBrix.Develop how to wire this font into a generated application.
  - A `.uprimarker` file that CodeBrix.Platform build pipelines use to
    discover font asset packages.
  - An MSBuild `.targets` file that prunes the redundant static fonts at
    consumer-build time on platforms without font-manifest support.

Provenance: this package is not a port of any upstream packaging
project — the packaging files and documentation are original CodeBrix
work; the only third-party material is the Merriweather, Noto Serif,
Noto Serif Armenian and Noto Serif Georgian `.ttf` binaries, which are
redistributed bit-for-bit unmodified (files were RENAMED only) with
full per-file attribution in the THIRD-PARTY-NOTICES.txt that ships
inside the package.


INSTALLATION
========================================================================

NuGet package id: CodeBrix.Platform.Fonts.Merriweather.OflLicenseForever

  dotnet add package CodeBrix.Platform.Fonts.Merriweather.OflLicenseForever

NuGet dependencies: NONE. The package has no PackageReference of its
own; it carries font binaries and MSBuild logic only.

License: OFL-1.1 (SIL Open Font License 1.1). The whole package —
packaging wrapper and bundled fonts alike — is published under that one
SPDX expression, and the package sets
`PackageRequireLicenseAcceptance` to true, so restore in an interactive
or license-checking pipeline will require accepting it. `OFL.txt` is
packed at the root of the nupkg.

The `.OflLicenseForever` suffix exists only on the NuGet package id, for
license disambiguation across the CodeBrix family. The assembly and the
`ms-appx:///` content root are both named
`CodeBrix.Platform.Fonts.Merriweather`, with no suffix.

Requirements and limits:

  * No native libraries, no OS-specific components; the package is
    platform-neutral content.
  * `ms-appx:///` URIs are resolved by the CodeBrix.Platform runtime.
    Outside a CodeBrix.Platform host the URIs mean nothing; a plain
    .NET 10 app can still open the `.ttf` files, but it has to locate
    them itself under the package's `lib/net10.0/...` folder in the
    NuGet cache.
  * The consumer-build prune (see COMMON PITFALLS) is driven by the
    `SupportsFontManifest` MSBuild property: when it is not `'true'`,
    the static font files are dropped from the app's assets and only the
    four variable fonts remain. The prune runs after the
    `_CodeBrixAddLibraryAssets` target, so it fires in a
    CodeBrix.Platform app build; a build that never runs that target
    keeps every font.

See also (sibling font packages, one per family, same shape):
CodeBrix.Platform.Fonts.OpenSans, CodeBrix.Platform.Fonts.Roboto,
CodeBrix.Platform.Fonts.RobotoMono, and — for musical notation glyphs
rather than text — CodeBrix.Platform.Fonts.NotoMusic. Each has its own
AGENT-README.txt at the root of its own repository and its own nupkg.


KEY NAMESPACES / USINGS
========================================================================

There is nothing to `using`. The package exposes no public managed
types, so no namespace import is ever required to consume it.

The identifier that matters is the content root of the `ms-appx:///`
URI space, which is the ASSEMBLY name:

  ms-appx:///CodeBrix.Platform.Fonts.Merriweather/Fonts/<file>.ttf

Every font in this package is addressed by one of those URIs. Examples:

  ms-appx:///CodeBrix.Platform.Fonts.Merriweather/Fonts/Merriweather.ttf
  ms-appx:///CodeBrix.Platform.Fonts.Merriweather/Fonts/Merriweather-Bold.ttf
  ms-appx:///CodeBrix.Platform.Fonts.Merriweather/Fonts/NotoSerif.ttf

Do NOT append a `#FamilyName` fragment to these URIs. CodeBrix.Platform
strips the fragment before resolving the font, so it buys nothing — and
on the value assigned to `FeatureConfiguration.Font.DefaultTextFontFamily`
it actively breaks the startup font-manifest preload, because the
".manifest" suffix the preload appends lands inside the URI fragment and
is then dropped.

The only managed identifier that appears in consumer code is
CodeBrix.Platform's own configuration entry point, which belongs to the
platform package rather than to this one:

  global::CodeBrix.Platform.UI.FeatureConfiguration.Font.DefaultTextFontFamily


FONT INVENTORY
========================================================================

The package ships 52 `.ttf` files plus 4 `.ttf.manifest` files.

PRIMARY FAMILY — Merriweather (25 files)
------------------------------------------------------------------------

Variable font (always present on every platform):

  Merriweather.ttf  — three axes: `wght` 300-900 (default 300), `wdth`
                      87-112 (default 100) and `opsz` 18-144 (default
                      18). Renamed, byte-for-byte, from the upstream
                      variable-font file
                      `Merriweather-VariableFont_opsz,wdth,wght.ttf`.

                      Note the DEFAULT instance is weight 300 (Light),
                      not Regular — set `FontWeight` explicitly when you
                      want Regular (400).

Static instances (used where fonts are resolved via the manifest):
six weights — Light (300), Regular (400), Medium (500), SemiBold (600),
Bold (700), ExtraBold (800) — in two styles (upright, Italic) across two
stretches:

  Normal stretch:         Merriweather-<Weight>[Italic].ttf
                          (12 files)
  SemiCondensed stretch:  Merriweather_SemiCondensed-<Weight>[Italic].ttf
                          (12 files)

  The upright Regular file is named `Merriweather-Regular.ttf`; the
  upright Italic of a weight drops the weight word only for Regular
  (`Merriweather-Italic.ttf`), matching the upstream naming.

  Merriweather publishes NO Condensed stretch upstream, which is why only
  two stretches ship here (Roboto ships three). Upstream also ships Black
  (900) static instances; those are intentionally NOT bundled — that
  weight remains reachable through the variable font. Merriweather
  publishes no Thin or ExtraLight statics at all.

  Optical size: upstream publishes five optical sizes (24, 36, 48, 96,
  120 pt). The manifest schema addresses fonts by style, weight and
  stretch ONLY — there is no optical-size dimension — so exactly one
  optical size is shipped: 24pt, the closest static to the variable
  font's own `opsz` default of 18. The upstream `_24pt` filename token is
  stripped so filenames match the family convention.

COMPANION FAMILIES (27 files)
------------------------------------------------------------------------

  NotoSerif.ttf + 12 statics          — supplies the GREEK script.
                                        Six weights, Normal stretch,
                                        upright and italic.
  NotoSerifArmenian.ttf + 6 statics   — supplies the ARMENIAN script.
                                        Six weights, Normal stretch,
                                        upright only.
  NotoSerifGeorgian.ttf + 6 statics   — supplies the GEORGIAN script.
                                        Six weights, Normal stretch,
                                        upright only.

  Neither Noto Serif Armenian nor Noto Serif Georgian has an italic face
  upstream, so italic text in those scripts renders upright. That is a
  known upstream limitation, not a packaging defect.

  All three companion variable fonts carry a `wght` axis of 100-900
  (default 400) and a `wdth` axis of 62.5-100 (default 100), so the
  variable file reaches weights and widths that the six static instances
  per family do not.

MANIFESTS
------------------------------------------------------------------------

  Merriweather.ttf.manifest       — 24 entries
  NotoSerif.ttf.manifest          — 12 entries
  NotoSerifArmenian.ttf.manifest  —  6 entries
  NotoSerifGeorgian.ttf.manifest  —  6 entries

Each manifest is a JSON OBJECT with a `fonts` array; each array element
has four string/number members:

  {
    "font_style":   "Normal" | "Italic",
    "font_weight":  300 | 400 | 500 | 600 | 700 | 800,
    "font_stretch": "Normal" | "SemiCondensed",
    "family_name":  "ms-appx:///CodeBrix.Platform.Fonts.Merriweather/Fonts/<file>.ttf"
  }

`family_name` holds the URI of the static file, not a typographic family
name — do not be misled by the member name.


SELECTING A WEIGHT, STYLE OR STRETCH
========================================================================

Set `FontFamily` to the DASH-FREE family URI (the variable font), then
express the face you want with the ordinary XAML text properties:

  FontFamily   -> ms-appx:///CodeBrix.Platform.Fonts.Merriweather/Fonts/Merriweather.ttf
  FontWeight   -> Light | Normal | Medium | SemiBold | Bold | ExtraBold
                  (the manifest matches the numeric weight behind each
                  name: Light 300, Normal 400, Medium 500, SemiBold 600,
                  Bold 700, ExtraBold 800)
  FontStyle    -> Normal | Italic
  FontStretch  -> Normal | SemiCondensed
  (and the same three properties on a <Run> inside a TextBlock)

Those three properties are exactly the triple the `.ttf.manifest` files
are keyed on — `font_style`, `font_weight`, `font_stretch` — so on a
platform that resolves fonts through the manifest (`SupportsFontManifest`
is `true`), the requested triple selects the matching STATIC file listed
in the manifest. Where the manifest is not used, the same properties
drive the variable font's own weight/width axes and the statics are not
in the payload at all (the build prunes them).

The complete set of triples the primary manifest can satisfy is the
cross product of the six weights, two styles and two stretches shown
above (24 entries). The companion manifests carry Normal stretch only,
and the Armenian and Georgian manifests carry upright only.

Combinations that are not in a manifest (Thin, ExtraLight, Black,
Condensed, or italic Armenian/Georgian) have no static instance to
resolve to; do not assume a specific file will be picked for them.


SCRIPT AND GLYPH COVERAGE
========================================================================

Counts below are codepoints present in each bundled VARIABLE font's
`cmap`. The static instances carry the same set: Merriweather-Regular,
Merriweather-Bold, Merriweather_SemiCondensed-Regular, NotoSerif-Regular
and NotoSerifArmenian-Regular each expose exactly the same codepoints as
their family's variable font.

Merriweather.ttf — 1,423 codepoints:

  Basic Latin 95, Latin-1 Supplement 95, Latin Extended-A 127,
  Latin Extended-B 180, IPA Extensions 41, Spacing Modifier Letters 30,
  Combining Diacritical Marks 45, Latin Extended Additional 247
  (covers Vietnamese), Cyrillic 239, Cyrillic Supplement 32,
  Cyrillic Extended-B 3, General Punctuation 46,
  Superscripts and Subscripts 19, Currency Symbols 25,
  Letterlike Symbols 11, Number Forms 16, Arrows 10,
  Mathematical Operators 18, Greek and Coptic 9.

  Those 9 Greek-and-Coptic codepoints are isolated symbol characters
  (the kind that appear in Latin text), NOT the Greek script. Treat
  Merriweather as Latin + Cyrillic only.

NotoSerif.ttf — 2,965 codepoints, including Greek and Coptic 121 and
  Greek Extended 233 (polytonic Greek), plus Latin and Cyrillic of its
  own.

NotoSerifArmenian.ttf — 430 codepoints, including Armenian 91.

NotoSerifGeorgian.ttf — 509 codepoints, including Georgian 88,
  Georgian Supplement 40 and Georgian Extended (Mtavruli) 46.

Nothing here covers Hebrew, Arabic, Indic scripts, CJK, emoji or
musical notation. The CodeBrix family never falls back to a system font,
so a codepoint outside the coverage above renders as `.notdef` — and all
four bundled families draw a `.notdef` BOX, so missing text shows as
tofu boxes rather than disappearing.


CODEBRIX-DEVELOP.JSON
========================================================================

`CODEBRIX-DEVELOP.json` is packed at the ROOT of the nupkg. It is the
font's self-description for CodeBrix.Develop's "New CodeBrix.Platform
Application" experience: the IDE reads it to learn how to wire this font
into a generated application, instead of carrying per-font logic. A
consumer reads it when it wants those same answers programmatically.

  schemaVersion     Always 1 today. A consumer that does not recognise
                    the value should decline the font with a clear
                    message rather than guess.
  packageId         Equals this package's NuGet package id.
  displayName       "Merriweather" — the typographic family name shown to
                    the user and written into generated source.
  fontFamilyUri     ms-appx:///CodeBrix.Platform.Fonts.Merriweather/Fonts/Merriweather.ttf
                    (no `#` fragment).
  resourceKey       "MerriweatherFont" — the App.xaml resource key a
                    generated application declares the family under.
  fallbackFontUris  Ordered URIs of the three companion fonts
                    (NotoSerif.ttf, NotoSerifArmenian.ttf,
                    NotoSerifGeorgian.ttf), consulted for codepoints the
                    primary font lacks. Absent or empty would mean the
                    package has no companions.
  keyboardLayouts   The software-keyboard layout ids this package's glyph
                    coverage supports, as the UNION across the primary
                    font and its companions. Ids absent from the list are
                    not supported; there is deliberately no "unsupported"
                    list, so the complement of the platform's layout set
                    is always the correct answer.

`keyboardLayouts` claims all 38 layouts the platform defines, including
`el`, `ka` and `hy` — which are delivered by the COMPANION fonts, not by
Merriweather. Those three depend on CodeBrix.Platform consulting
`fallbackFontUris` when the primary font lacks a glyph.


CORE API REFERENCE
========================================================================

This package has no public managed API — the assembly deliberately
exports zero public types (a test pins that). The complete consumer
surface is four things:

  1. FONT URIs — used as `FontFamily` values in XAML, in code that
     builds XAML element trees, or as the CodeBrix.Platform default:

       global::CodeBrix.Platform.UI.FeatureConfiguration.Font.DefaultTextFontFamily =
           "ms-appx:///CodeBrix.Platform.Fonts.Merriweather/Fonts/Merriweather.ttf";

     Assign the DASH-FREE variable-font URI here, with no `#` fragment.

  2. THE APP.XAML RESOURCE KEY — `MerriweatherFont`, the key under which
     a generated CodeBrix.Platform application declares the family (see
     COMPLETE EXAMPLES). The key is data in CODEBRIX-DEVELOP.json, not a
     compiled constant.

  3. THE MSBUILD TARGET the package injects into the consumer build,
     from `buildTransitive/net10.0/` (its file name matches the NuGet
     package id so NuGet's auto-import convention picks it up):

       <Target Name="CodeBrixRemoveUnusedMerriweather"
               AfterTargets="_CodeBrixAddLibraryAssets">

     When `$(SupportsFontManifest)` is not `'true'`, it removes the
     dash-bearing static font files from the app's asset items, leaving
     the four variable fonts. Set `SupportsFontManifest` to `true` in a
     head project to keep the statics.

  4. CODEBRIX-DEVELOP.json — documented above.

If a future iteration of this package exposes a managed API, it will
live under the `CodeBrix.Platform.Fonts.Merriweather` root namespace and
be documented here.


WHAT IS IN THE NUGET PACKAGE
========================================================================

Consumer-visible layout of the produced nupkg:

  buildTransitive/net10.0/CodeBrix.Platform.Fonts.Merriweather.OflLicenseForever.targets
  lib/net10.0/CodeBrix.Platform.Fonts.Merriweather.dll
  lib/net10.0/CodeBrix.Platform.Fonts.Merriweather.uprimarker
  lib/net10.0/CodeBrix.Platform.Fonts.Merriweather/Fonts/*.ttf
  lib/net10.0/CodeBrix.Platform.Fonts.Merriweather/Fonts/*.ttf.manifest
  AGENT-README.txt
  CODEBRIX-DEVELOP.json
  README.md
  OFL.txt
  THIRD-PARTY-NOTICES.txt
  icon-codebrix-128.png

The `lib/net10.0/CodeBrix.Platform.Fonts.Merriweather/Fonts/` folder name
is load-bearing: the `ms-appx:///CodeBrix.Platform.Fonts.Merriweather/
Fonts/...` URIs resolve relative to the assembly name, so the folder and
the assembly always carry the same name.


COMPLETE EXAMPLES
========================================================================

1. A TextBlock in the family's default (variable) font
------------------------------------------------------------------------

    <TextBlock Text="Hello, world."
               FontFamily="ms-appx:///CodeBrix.Platform.Fonts.Merriweather/Fonts/Merriweather.ttf" />

2. Weight, style and stretch — the manifest-driven selection
------------------------------------------------------------------------

    <StackPanel>

      <!-- SemiBold upright, Normal stretch  ->  Merriweather-SemiBold.ttf -->
      <TextBlock Text="Section heading"
                 FontFamily="ms-appx:///CodeBrix.Platform.Fonts.Merriweather/Fonts/Merriweather.ttf"
                 FontWeight="SemiBold" />

      <!-- Bold italic, Normal stretch  ->  Merriweather-BoldItalic.ttf -->
      <TextBlock Text="Emphatic sentence"
                 FontFamily="ms-appx:///CodeBrix.Platform.Fonts.Merriweather/Fonts/Merriweather.ttf"
                 FontWeight="Bold"
                 FontStyle="Italic" />

      <!-- Light upright, SemiCondensed  ->
           Merriweather_SemiCondensed-Light.ttf -->
      <TextBlock Text="Narrow caption text"
                 FontFamily="ms-appx:///CodeBrix.Platform.Fonts.Merriweather/Fonts/Merriweather.ttf"
                 FontWeight="Light"
                 FontStretch="SemiCondensed" />

    </StackPanel>

  FontWeight / FontStyle / FontStretch on the element (or on a <Run>) are
  the values matched against `font_weight` / `font_style` /
  `font_stretch` in `Merriweather.ttf.manifest`.

3. Mixed weights inside one paragraph, with <Run>
------------------------------------------------------------------------

    <TextBlock FontFamily="ms-appx:///CodeBrix.Platform.Fonts.Merriweather/Fonts/Merriweather.ttf">
      <Run Text="Normal body text, " />
      <Run Text="bold words, " FontWeight="Bold" />
      <Run Text="and an italic aside." FontStyle="Italic" />
    </TextBlock>

4. App.xaml — declare the family once under the descriptor's key
------------------------------------------------------------------------

    <Application x:Class="MyApp.App"
                 xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                 xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
      <Application.Resources>
        <ResourceDictionary>

          <!-- Key name comes from CODEBRIX-DEVELOP.json "resourceKey" -->
          <FontFamily x:Key="MerriweatherFont">ms-appx:///CodeBrix.Platform.Fonts.Merriweather/Fonts/Merriweather.ttf</FontFamily>

          <Style x:Key="BodyTextStyle" TargetType="TextBlock">
            <Setter Property="FontFamily" Value="{StaticResource MerriweatherFont}" />
            <Setter Property="FontSize" Value="15" />
          </Style>

        </ResourceDictionary>
      </Application.Resources>
    </Application>

  and then, in any page:

    <TextBlock Text="Body copy"
               FontFamily="{StaticResource MerriweatherFont}" />
    <TextBlock Text="Styled body copy"
               Style="{StaticResource BodyTextStyle}" />

5. Make Merriweather the application-wide default text font (C#)
------------------------------------------------------------------------

    // Run this before the first UI element is created — typically at the
    // top of the App constructor, before InitializeComponent().
    global::CodeBrix.Platform.UI.FeatureConfiguration.Font.DefaultTextFontFamily =
        "ms-appx:///CodeBrix.Platform.Fonts.Merriweather/Fonts/Merriweather.ttf";

6. Greek, Armenian or Georgian text
------------------------------------------------------------------------

Merriweather itself has no glyphs for those scripts. Either rely on the
platform's fallback chain (the three companion URIs are listed in
CODEBRIX-DEVELOP.json's `fallbackFontUris`), or address the companion
font directly on the run that needs it:

    <TextBlock FontFamily="ms-appx:///CodeBrix.Platform.Fonts.Merriweather/Fonts/Merriweather.ttf">
      <Run Text="Greek follows: " />
      <Run Text="&#x03B1;&#x03B2;&#x03B3;"
           FontFamily="ms-appx:///CodeBrix.Platform.Fonts.Merriweather/Fonts/NotoSerif.ttf" />
    </TextBlock>

    <!-- Armenian, weight-matched to the surrounding text -->
    <TextBlock Text="&#x0531;&#x0532;&#x0533;"
               FontFamily="ms-appx:///CodeBrix.Platform.Fonts.Merriweather/Fonts/NotoSerifArmenian.ttf"
               FontWeight="SemiBold" />


MINIMUM VIABLE PROJECT
========================================================================

MyApp.csproj (the font package is the only reference this needs; a real
CodeBrix.Platform head project also references the platform packages):

    <Project Sdk="Microsoft.NET.Sdk">

      <PropertyGroup>
        <TargetFramework>net10.0</TargetFramework>
        <!-- Keep the static instances in the app payload. When this is
             not 'true', the package's .targets prunes them and only the
             four variable fonts ship. The prune runs after the
             _CodeBrixAddLibraryAssets target, so it only fires in a
             build that runs that target. -->
        <SupportsFontManifest>true</SupportsFontManifest>
      </PropertyGroup>

      <ItemGroup>
        <PackageReference Include="CodeBrix.Platform.Fonts.Merriweather.OflLicenseForever" />
      </ItemGroup>

    </Project>

App.xaml — as in COMPLETE EXAMPLES section 4.

MainPage.xaml:

    <Page x:Class="MyApp.MainPage"
          xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
          xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
      <StackPanel Padding="24" Spacing="8">
        <TextBlock Text="Merriweather"
                   FontFamily="{StaticResource MerriweatherFont}"
                   FontWeight="Bold"
                   FontSize="28" />
        <TextBlock Text="A serif face with Latin and Cyrillic coverage."
                   FontFamily="{StaticResource MerriweatherFont}"
                   FontSize="15" />
      </StackPanel>
    </Page>

Outside a CodeBrix.Platform host there is no `ms-appx:///` resolver. A
plain .NET 10 program that wants the bytes must find the file itself in
the restored package folder, for example:

    <nuget-cache>/codebrix.platform.fonts.merriweather.ofllicenseforever/
        <version>/lib/net10.0/CodeBrix.Platform.Fonts.Merriweather/Fonts/
        Merriweather.ttf


PERFORMANCE TIPS
========================================================================

  * This is a static asset carrier: there is no managed code on any hot
    path. The only cost that matters is PAYLOAD SIZE and font-file
    loading.

  * The bundled fonts total about 39 MB on disk: roughly 32 MB of static
    instances and 6.9 MB across the four variable fonts. On platforms
    where `SupportsFontManifest` is not `true`, the build prune drops the
    statics, so the app carries only the variable fonts.

  * Reference ONE family URI (the dash-free variable font) and vary
    FontWeight / FontStyle / FontStretch, rather than naming many static
    files directly. It keeps the referenced-font set small and lets the
    platform load only the faces actually used.

  * Prefer the primary font for body text and reach for a companion only
    on the runs that need Greek, Armenian or Georgian; each additional
    directly-referenced family is another font file to load.


COMMON PITFALLS TO AVOID
========================================================================

  * NEVER add a `#FamilyName` fragment to a font URI. CodeBrix.Platform
    strips it during font resolution, and on `DefaultTextFontFamily` it
    silently disables the startup manifest preload (the appended
    ".manifest" lands inside the fragment and is dropped by
    `Uri.PathAndQuery`).

  * Do not hard-code a DASH-BEARING static URI (e.g.
    `.../Fonts/Merriweather-Bold.ttf`) in code or XAML that must work
    everywhere. Those files are exactly what the package's `.targets`
    prunes when `SupportsFontManifest` is not `'true'`, so the reference
    resolves on some heads and not on others. Use the dash-free URI plus
    `FontWeight="Bold"` instead. The four variable fonts are never
    pruned, which is why the companion families are named without a dash.

  * Do not expect Greek, Armenian or Georgian from Merriweather itself —
    it has no glyphs for them (see SCRIPT AND GLYPH COVERAGE). That is
    what the three companion families are for, and why the descriptor
    lists `fallbackFontUris`.

  * There is no system-font fallback anywhere in the CodeBrix family.
    Uncovered codepoints do not silently borrow a font from the OS; they
    render as `.notdef`, and every font in this package draws a box for
    it — so unsupported text shows as tofu boxes. Check coverage before
    assuming a rendering bug.

  * Merriweather's copyright statement DOES declare a Reserved Font Name
    ("Merriweather"), so SIL OFL 1.1 condition 3 applies: do not alter
    the font bytes or the internal name tables, and do not redistribute a
    modified font under that name. The three Noto Serif families declare
    no Reserved Font Name.

  * Only ONE optical size ships as static instances (24pt). The manifest
    has no optical-size dimension, so additional optical sizes could not
    be addressed even if they were added — they would only add roughly
    100 MB of payload.

  * Requesting Thin, ExtraLight, Black or a Condensed stretch has no
    matching static instance in this package.

  * `ms-appx:///` is a CodeBrix.Platform concept, not a .NET one. In a
    console app or unit test the URI will not resolve; locate the file on
    disk instead.


WHAT THIS PACKAGE DOES NOT DO
========================================================================

  * It exposes no public managed types, no font-loading helper, no
    typeface API and no glyph-metrics API. Referencing it gives you
    files, not objects.
  * It does not install fonts into the operating system, and does not
    register fonts with any OS font service.
  * It does not ship Black (900), Thin or ExtraLight static instances,
    and there is no Condensed stretch — Merriweather publishes none.
  * It does not ship italics for Armenian or Georgian (upstream has
    none), so italic text in those scripts renders upright.
  * It does not ship optical sizes other than 24pt as statics.
  * It does not cover Hebrew, Arabic, Indic scripts, CJK, emoji or
    musical-notation symbols. For musical notation, see the sibling
    package CodeBrix.Platform.Fonts.NotoMusic.
  * It does not fall back to a system font, and it cannot make one
    available.
  * It does not implement the font-fallback resolution itself — the
    descriptor merely NAMES the companion URIs; consulting them is
    CodeBrix.Platform's job.
  * It has no runtime dependency on CodeBrix.Platform: nothing stops a
    non-platform project from referencing the package for the font files
    alone.


WORKING EXAMPLES ON GITHUB
========================================================================

The package's own test suite is the executable specification of every
claim above — entry counts, URI shapes, descriptor contents and the
MSBuild target's behaviour:

  https://github.com/ellisnet/CodeBrix.Platform.Fonts.Merriweather/tree/main/tests/CodeBrix.Platform.Fonts.Merriweather.Tests

  ContentManifestTests.cs   — the four manifests: entry counts (24/12/
                              6/6), the six weights, Normal+Italic and
                              Normal+SemiCondensed on the primary
                              family, upright-only companions, and that
                              every `family_name` URI is rooted at
                              `ms-appx:///CodeBrix.Platform.Fonts.Merriweather/Fonts/`
                              and names a file that exists.
  ContentFilePresenceTests.cs — that all 52 `.ttf` files ship, and that
                              no `_24pt`-style optical-size token
                              survives in any filename.
  DescriptorTests.cs        — CODEBRIX-DEVELOP.json: schemaVersion,
                              packageId, displayName "Merriweather",
                              resourceKey "MerriweatherFont", the
                              no-`#`-fragment rule on every URI, and the
                              three companion fallback URIs.
  TargetsFileTests.cs       — that the `.targets` declares
                              `CodeBrixRemoveUnusedMerriweather`, hooks
                              `AfterTargets="_CodeBrixAddLibraryAssets"`,
                              carries the `SupportsFontManifest`
                              condition, and never removes a variable
                              font.
  AssemblyMetadataTests.cs  — that the assembly is named
                              `CodeBrix.Platform.Fonts.Merriweather`,
                              targets .NET 10 and exports no public
                              types.

Repository root (README.md has short XAML/C# snippets too):

  https://github.com/ellisnet/CodeBrix.Platform.Fonts.Merriweather


QUICK REFERENCE CARD
========================================================================

Package id .... CodeBrix.Platform.Fonts.Merriweather.OflLicenseForever
License ....... OFL-1.1 (acceptance required)
Dependencies .. none          Target ........ .NET 10 or later
Public types .. none          Resource key .. MerriweatherFont
URI prefix .... ms-appx:///CodeBrix.Platform.Fonts.Merriweather/Fonts/

FAMILY URIs (dash-free — always present, never pruned)

  <prefix>Merriweather.ttf           Latin + Cyrillic, variable
  <prefix>NotoSerif.ttf              Greek companion, variable
  <prefix>NotoSerifArmenian.ttf      Armenian companion, variable
  <prefix>NotoSerifGeorgian.ttf      Georgian companion, variable

MERRIWEATHER STATIC INSTANCES (pruned unless SupportsFontManifest=true)

  weight  upright (Normal stretch)   italic (Normal stretch)
  ------  -------------------------  --------------------------------
  300     Merriweather-Light.ttf     Merriweather-LightItalic.ttf
  400     Merriweather-Regular.ttf   Merriweather-Italic.ttf
  500     Merriweather-Medium.ttf    Merriweather-MediumItalic.ttf
  600     Merriweather-SemiBold.ttf  Merriweather-SemiBoldItalic.ttf
  700     Merriweather-Bold.ttf      Merriweather-BoldItalic.ttf
  800     Merriweather-ExtraBold.ttf Merriweather-ExtraBoldItalic.ttf

  SemiCondensed stretch: the same 12 names with the prefix
  `Merriweather_SemiCondensed-` in place of `Merriweather-`
  (e.g. Merriweather_SemiCondensed-SemiBoldItalic.ttf).

COMPANION STATIC INSTANCES (Normal stretch only)

  NotoSerif-{Light|Regular|Medium|SemiBold|Bold|ExtraBold}.ttf
  NotoSerif-{Light|Medium|SemiBold|Bold|ExtraBold}Italic.ttf
    plus NotoSerif-Italic.ttf for weight 400
  NotoSerifArmenian-{Light|Regular|Medium|SemiBold|Bold|ExtraBold}.ttf
  NotoSerifGeorgian-{Light|Regular|Medium|SemiBold|Bold|ExtraBold}.ttf

XAML PROPERTY -> MANIFEST MEMBER

  FontStyle   -> font_style    ("Normal" | "Italic")
  FontWeight  -> font_weight   (300 | 400 | 500 | 600 | 700 | 800)
  FontStretch -> font_stretch  ("Normal" | "SemiCondensed")

MSBuild

  Target ..... CodeBrixRemoveUnusedMerriweather
  Hook ....... AfterTargets="_CodeBrixAddLibraryAssets"
  Property ... SupportsFontManifest ('true' keeps the static instances)

RULES

  * No `#FamilyName` fragment on any font URI, ever.
  * Prefer the dash-free URI + FontWeight/FontStyle/FontStretch.
  * No system-font fallback; uncovered codepoints render blank.
