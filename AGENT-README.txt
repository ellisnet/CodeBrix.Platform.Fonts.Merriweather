========================================================================
AGENT-README: CodeBrix.Platform.Fonts.Merriweather
A Comprehensive Guide for AI Coding Agents
========================================================================


OVERVIEW
========================================================================

CodeBrix.Platform.Fonts.Merriweather is a .NET 10 redistribution of the
Merriweather font family, packaged for the CodeBrix family. It supplies
the Merriweather variable font and a curated set of static instances as
build-time content assets for CodeBrix.Platform-forked applications, and
is equally usable as a plain content-files NuGet in any .NET 10 project.

Merriweather covers the Latin and Cyrillic scripts but NOT Greek,
Armenian or Georgian. This package therefore also bundles three Noto
Serif COMPANION families that supply those scripts in a matching serif
design. That is the one structural difference from the sibling
CodeBrix.Platform.Fonts.OpenSans and CodeBrix.Platform.Fonts.Roboto
packages, and it is the thing to understand before changing anything
here.

The library has effectively no managed code: the assembly is a metadata-
only .NET 10 DLL whose sole purpose is to host the bundled font content
files. The interesting payload lives in:

  - 52 `.ttf` font files (4 variable + 48 static) under
    lib/net10.0/CodeBrix.Platform.Fonts.Merriweather/Fonts/ inside the
    nupkg.
  - Four `.ttf.manifest` JSON files (one per family) mapping
    font_style/font_weight/font_stretch triples to the matching static
    font file path.
  - A `CODEBRIX-DEVELOP.json` descriptor at the package root that tells
    CodeBrix.Develop how to wire this font into a generated application.
  - A `.uprimarker` file that CodeBrix.Platform build pipelines use to
    discover UPRI-bearing font asset packages.
  - An MSBuild `.targets` file under buildTransitive/net10.0/ that hooks
    into the CodeBrix.Platform `_CodeBrixAddLibraryAssets` target and
    prunes the redundant static fonts at consumer-build time, depending
    on the `SupportsFontManifest` MSBuild property — while always keeping
    all four variable fonts present.


INSTALLATION
========================================================================

NuGet package: CodeBrix.Platform.Fonts.Merriweather.OflLicenseForever

  dotnet add package CodeBrix.Platform.Fonts.Merriweather.OflLicenseForever

The library namespace inside the assembly is
`CodeBrix.Platform.Fonts.Merriweather` (without the `.OflLicenseForever`
suffix; that suffix exists only on the NuGet PackageId for license-
disambiguation across the CodeBrix family).

Target framework: .NET 10.0 or higher.


KEY NAMESPACE
========================================================================

The library exposes no public managed types in its first iteration — the
assembly is metadata-only. Consumers reference the bundled font content
files via `ms-appx:///` URIs rooted at the assembly content folder:

  ms-appx:///CodeBrix.Platform.Fonts.Merriweather/Fonts/Merriweather.ttf
  ms-appx:///CodeBrix.Platform.Fonts.Merriweather/Fonts/Merriweather-Bold.ttf
  ms-appx:///CodeBrix.Platform.Fonts.Merriweather/Fonts/NotoSerif.ttf
  ...etc.

Do NOT append a `#FamilyName` fragment to these URIs. CodeBrix.Platform
strips the fragment before resolving the font, so it buys nothing — and
on the value assigned to `FeatureConfiguration.Font.DefaultTextFontFamily`
it actively breaks the startup font-manifest preload, because the
".manifest" suffix the preload appends lands inside the URI fragment and
is then dropped.


FONT INVENTORY
========================================================================

The package ships 52 `.ttf` files plus 4 `.ttf.manifest` files.

PRIMARY FAMILY — Merriweather (25 files)

Variable font (always present on every platform):
  Merriweather.ttf  — covers the weight axis (300-900), the width axis
                      and the optical-size axis. Renamed, byte-for-byte,
                      from the upstream variable-font file
                      `Merriweather-VariableFont_opsz,wdth,wght.ttf`.

Static fonts (used where fonts are resolved via the static manifest):
  Six weights (Light, Regular, Medium, SemiBold, Bold, ExtraBold)
  in two styles (Normal, Italic) across two stretches:
    - Normal stretch:        Merriweather-{Weight}{Italic?}.ttf         (12 files)
    - SemiCondensed stretch: Merriweather_SemiCondensed-{Weight}{Italic?}.ttf (12 files)

  Note: Merriweather publishes NO Condensed stretch upstream, which is
  why only two stretches ship here (Roboto ships three). Upstream also
  ships Black (900) static instances; those are intentionally NOT bundled
  (that weight remains reachable through the variable font). Merriweather
  publishes no Thin or ExtraLight statics at all.

  Note on optical size: upstream publishes five optical sizes (24, 36,
  48, 96, 120 pt). The manifest schema addresses fonts by style, weight
  and stretch ONLY — there is no optical-size dimension — so exactly one
  optical size is shipped: 24pt, the closest static to the variable
  font's own `opsz` default of 18. The upstream `_24pt` filename token is
  stripped so filenames match the family convention.

COMPANION FAMILIES (27 files)

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

Manifests:
  Merriweather.ttf.manifest       — 24 entries
  NotoSerif.ttf.manifest          — 12 entries
  NotoSerifArmenian.ttf.manifest  —  6 entries
  NotoSerifGeorgian.ttf.manifest  —  6 entries

  Each is a JSON object with a `fonts` array mapping
  {font_style, font_weight, font_stretch} triples to the matching static
  font file's `ms-appx:///` URI.


CODEBRIX-DEVELOP.JSON
========================================================================

`CODEBRIX-DEVELOP.json` sits at the repository root and is packed to the
root of the nupkg. It is the font's self-description for CodeBrix.Develop's
"New CodeBrix.Platform Application" experience: the IDE reads it to learn
how to wire this font into a generated application, instead of carrying
per-font swap logic of its own.

  schemaVersion     Always 1 today. A consumer that does not recognise
                    the value should decline the font with a clear
                    message rather than guess.
  packageId         Must equal this package's NuGet PackageId.
  displayName       The typographic family name shown to the user, and
                    the authoritative value written into generated source.
  fontFamilyUri     The ms-appx URI of the primary font. No `#` fragment.
  resourceKey       The App.xaml resource key a generated application
                    uses (`MerriweatherFont`).
  fallbackFontUris  Ordered ms-appx URIs of the companion fonts, consulted
                    for codepoints the primary font lacks. Absent or empty
                    means the package has no companions.
  keyboardLayouts   The software-keyboard layout ids this package's glyph
                    coverage supports, as the UNION across the primary
                    font and its companions. Ids absent from this list are
                    not supported; there is deliberately no "unsupported"
                    list, so the complement of the platform's layout set
                    is always the correct answer.

The array is generated, not hand-written — see PROVENANCE below.

IMPORTANT: `keyboardLayouts` currently claims all 38 layouts, including
`el`, `ka` and `hy`, which are delivered by the companion fonts. Those
three require CodeBrix.Platform to consult `fallbackFontUris` when the
primary font lacks a glyph. If you are reading this before that support
shipped, the claim runs ahead of the runtime by design — it was published
deliberately, with the platform work following immediately.


CORE API REFERENCE
========================================================================

This library has no public managed API. Consumers interact with it only
through:

  1. NuGet content paths
     (`ms-appx:///CodeBrix.Platform.Fonts.Merriweather/Fonts/...`) used as
     `FontFamily` values in XAML or in code that constructs XAML element
     trees, or by setting the CodeBrix.Platform default font:

       global::CodeBrix.Platform.UI.FeatureConfiguration.Font.DefaultTextFontFamily =
           "ms-appx:///CodeBrix.Platform.Fonts.Merriweather/Fonts/Merriweather.ttf";

  2. The MSBuild `.targets` file under buildTransitive/net10.0/
     `CodeBrix.Platform.Fonts.Merriweather.OflLicenseForever.targets`,
     whose on-disk filename matches the NuGet PackageId so that NuGet's
     auto-import convention (NU5129) picks it up in consumer builds. It
     contains the target:

       <Target Name="CodeBrixRemoveUnusedMerriweather"
               AfterTargets="_CodeBrixAddLibraryAssets">

     On platforms that do not support the font manifest, this target
     removes the static fonts (leaving only the four variable fonts).

  3. `CODEBRIX-DEVELOP.json`, read by CodeBrix.Develop (see above).

If a future iteration of this library exposes a managed API, it will live
under the `CodeBrix.Platform.Fonts.Merriweather` root namespace and be
documented in this file.


ARCHITECTURE
========================================================================

Repository layout:

  CodeBrix.Platform.Fonts.Merriweather/
    src/CodeBrix.Platform.Fonts.Merriweather/
      CodeBrix.Platform.Fonts.Merriweather.csproj
      InternalsVisibleTo.cs
      CodeBrix.Platform.Fonts.Merriweather.uprimarker   (empty file)
      buildTransitive/
        net10.0/
          CodeBrix.Platform.Fonts.Merriweather.OflLicenseForever.targets
      Fonts/
        Merriweather.ttf
        Merriweather.ttf.manifest
        Merriweather-{Light|Regular|Medium|SemiBold|Bold|ExtraBold}{Italic?}.ttf
        Merriweather_SemiCondensed-{Weight}{Italic?}.ttf
        NotoSerif.ttf / NotoSerif.ttf.manifest / NotoSerif-{Weight}{Italic?}.ttf
        NotoSerifArmenian.ttf / .ttf.manifest / NotoSerifArmenian-{Weight}.ttf
        NotoSerifGeorgian.ttf / .ttf.manifest / NotoSerifGeorgian-{Weight}.ttf
    tests/CodeBrix.Platform.Fonts.Merriweather.Tests/
      CodeBrix.Platform.Fonts.Merriweather.Tests.csproj
      AssemblyMetadataTests.cs
      ContentFilePresenceTests.cs
      ContentManifestTests.cs
      DescriptorTests.cs
      TargetsFileTests.cs
      TestAssetPaths.cs
    AGENT-README.txt
    CODEBRIX-DEVELOP.json
    LICENSE                  (SIL OFL 1.1)
    OFL.txt                  (SIL OFL 1.1; identical to LICENSE)
    README.md
    THIRD-PARTY-NOTICES.txt

Inside the produced NuGet (.nupkg), the file layout is:
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

The `lib/net10.0/CodeBrix.Platform.Fonts.Merriweather/Fonts/` content
layout is load-bearing: the
`ms-appx:///CodeBrix.Platform.Fonts.Merriweather/Fonts/...` URIs that
consumers reference resolve relative to the assembly name, so if the
assembly is renamed the content folder must be renamed in lockstep.


CODING CONVENTIONS (CodeBrix family)
========================================================================

This repository follows every CodeBrix family convention. Most are
inherited from the standard library scaffold; key points:

  * Target framework: net10.0 only. No multi-targeting.
  * Nullable reference types (NRT): OFF (do not set <Nullable>enable</Nullable>).
    No `?` annotations on reference types; no `!` null-forgiveness operator.
    Value-type nullables (`int?`, `DateOnly?`, etc.) are fine.
  * No global usings.
  * `<GenerateDocumentationFile>true</GenerateDocumentationFile>` is on.
    Every public/protected member of a public type needs an XML doc
    comment. CS1591 is fixed at source, never suppressed. (In this
    library's first iteration there are no public types, so CS1591
    is trivially clean.)
  * Tests use xUnit v3 + SilverAssertions; coverlet.collector for
    coverage; `TestContext.Current.CancellationToken` is threaded through
    any cancellable call inside a test.
  * No project-level warning suppression (`<NoWarn>`, `<WarningLevel>0</>`,
    `<TreatWarningsAsErrors>false</>`, etc. are all forbidden).
  * The whole package — wrapper code and bundled fonts alike — is licensed
    under SIL OFL 1.1; the csproj `<PackageLicenseExpression>` is `OFL-1.1`.
    The `<Copyright>` line preserves the upstream font attribution for all
    four bundled families.

For the full list of family conventions see CODEBRIX_LIBRARY_OBSERVATIONS.txt
in the CodeBrix.Library.Dev-private repo.


TESTING
========================================================================

Tests live under tests/CodeBrix.Platform.Fonts.Merriweather.Tests/.
Run with:

  dotnet test CodeBrix.Platform.Fonts.Merriweather.slnx

The test suite covers:

  * Manifest JSON: that all four `.ttf.manifest` files deserialize
    cleanly, carry the expected entry counts (24/12/6/6), cover the six
    weights, and that every entry's family_name path is rooted at
    `ms-appx:///CodeBrix.Platform.Fonts.Merriweather/Fonts/` and points at
    a file that exists on disk. Also that the Armenian and Georgian
    manifests are upright-only, so that limitation stays a decision
    rather than an accident.
  * Content-file presence: that all 52 `.ttf` files exist next to the test
    assembly's expected build-output font folder (resolved via
    `AppContext.BaseDirectory` + `TestAssets/Fonts/`, centralized in
    `TestAssetPaths`), and that no `_24pt`-style optical-size token
    survives in any filename.
  * Descriptor: that CODEBRIX-DEVELOP.json declares schemaVersion 1, its
    packageId matches the published PackageId, its fontFamilyUri and every
    fallbackFontUri carry no `#` fragment and point at fonts this package
    actually ships, and that keyboardLayouts has no duplicates and claims
    the three scripts the companions exist to supply.
  * Assembly metadata: that the produced library assembly is named
    `CodeBrix.Platform.Fonts.Merriweather` and exports no public types.
  * .targets file: that the buildTransitive .targets file is present, that
    it declares the `CodeBrixRemoveUnusedMerriweather` MSBuild target, that
    it hooks `AfterTargets="_CodeBrixAddLibraryAssets"`, and that it never
    removes any of the four variable fonts.


PROVENANCE
========================================================================

This package is not a port of any upstream packaging project. The
`.csproj`, `.targets`, `.ttf.manifest` files, `CODEBRIX-DEVELOP.json`,
`.uprimarker`, and documentation are original CodeBrix-family files. The
only third-party material is the Merriweather and Noto Serif `.ttf` font
binaries, which are redistributed bit-for-bit unmodified. Their per-file
provenance and the SIL OFL 1.1 terms are recorded in
THIRD-PARTY-NOTICES.txt (binary `.ttf` files cannot carry an inline
provenance comment).

The `keyboardLayouts` array in CODEBRIX-DEVELOP.json is GENERATED, not
hand-written: it is computed by intersecting each software-keyboard
layout's required character set (from the layout definitions in
CodeBrix.Platform) against the `cmap` of every font this package ships,
then taking the union across the primary font and its companions. Nothing
in this repository's build reads CodeBrix.Platform — the array is computed
by a developer-run tool and checked in as data. Regenerate it whenever the
platform's layout set changes or this package's font set changes.


KNOWN GOTCHAS
========================================================================

  * `ms-appx:///` URIs are resolved by the CodeBrix.Platform runtime, not
    by .NET itself. Outside a CodeBrix.Platform host, those URIs won't
    resolve. Plain .NET 10 console / test apps that reference this package
    can still access the .ttf files via the package's on-disk location
    (`<nuget-cache>/codebrix.platform.fonts.merriweather.ofllicenseforever/<version>/lib/net10.0/CodeBrix.Platform.Fonts.Merriweather/Fonts/...`),
    but they have to do that lookup themselves.

  * NEVER add a `#FamilyName` fragment to a font URI in this package's
    documentation or descriptor. CodeBrix.Platform strips it during font
    resolution, and on `DefaultTextFontFamily` it silently disables the
    startup manifest preload (the appended ".manifest" lands inside the
    fragment and is dropped by `Uri.PathAndQuery`).

  * The .targets file hooks `AfterTargets="_CodeBrixAddLibraryAssets"` —
    the asset target defined by the CodeBrix.Platform UI build tasks. If
    that internal MSBuild target name ever changes again, this .targets
    file must be updated in lockstep — otherwise the conditional pruning
    of static fonts will silently stop firing.

  * The four variable fonts are deliberately never pruned. For
    Merriweather.ttf that is the usual reason (consumers reference it by
    direct path). For the three companions it matters MORE: they are the
    only source of Greek, Armenian and Georgian in this package, so
    pruning them would silently drop three scripts rather than merely
    degrade weights. The prune matches only dash-bearing filenames, which
    is why the companion variable fonts are named without a dash.

  * Merriweather's copyright statement DOES declare a Reserved Font Name
    ("Merriweather"), unlike Roboto's. SIL OFL 1.1 condition 3 therefore
    applies: do not alter the font bytes or the internal name tables. File
    renames (as done for the variable font and the `_24pt` statics) are
    fine — they do not create a Modified Version. The Noto Serif families
    declare no Reserved Font Name.

  * Only one optical size of Merriweather ships as statics. If someone
    "helpfully" adds the 36/48/96/120pt sets, the manifest cannot address
    them (it has no opsz dimension) and the package grows by ~100 MB for
    no benefit.
