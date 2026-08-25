========================================================================
MAINTAINER-README: CodeBrix.Platform.Fonts.Merriweather
Notes for people and agents MAINTAINING this repository — not for
package consumers
========================================================================


PURPOSE AND SCOPE
========================================================================

This repository produces exactly one NuGet package:

  CodeBrix.Platform.Fonts.Merriweather.OflLicenseForever
      built from src/CodeBrix.Platform.Fonts.Merriweather/
      consumer documentation: AGENT-README.txt (repository root)

The package is a font asset carrier: a metadata-only .NET 10 assembly
plus 52 `.ttf` files, 4 `.ttf.manifest` files, a CODEBRIX-DEVELOP.json
descriptor, a `.uprimarker` marker and a buildTransitive `.targets` file.
There is no product source code to maintain — the maintenance surface is
the font set, the manifests, the descriptor, the `.targets` file and the
tests that pin all of them.

If you are consuming the package rather than changing this repository,
read AGENT-README.txt instead and stop here.


REPOSITORY LAYOUT
========================================================================

  CodeBrix.Platform.Fonts.Merriweather/
    CodeBrix.Platform.Fonts.Merriweather.slnx
    AGENT-README.txt            (consumer docs; packed into the nupkg)
    MAINTAINER-README.txt       (this file; NOT packed)
    EXTRAS-README.txt           (NOT packed)
    README-INDEX.txt            (NOT packed)
    README.md                   (GitHub + nuget.org; packed)
    CODEBRIX-DEVELOP.json       (packed to the nupkg root)
    LICENSE                     (SIL OFL 1.1)
    OFL.txt                     (SIL OFL 1.1; identical to LICENSE; packed)
    THIRD-PARTY-NOTICES.txt     (packed)
    icon-codebrix-128.png       (packed)
    src/CodeBrix.Platform.Fonts.Merriweather/
      CodeBrix.Platform.Fonts.Merriweather.csproj
      InternalsVisibleTo.cs
      CodeBrix.Platform.Fonts.Merriweather.uprimarker      (empty file)
      buildTransitive/net10.0/
        CodeBrix.Platform.Fonts.Merriweather.OflLicenseForever.targets
      Fonts/
        Merriweather.ttf + Merriweather.ttf.manifest
        Merriweather-<Weight>[Italic].ttf                  (12)
        Merriweather_SemiCondensed-<Weight>[Italic].ttf    (12)
        NotoSerif.ttf + manifest + 12 statics
        NotoSerifArmenian.ttf + manifest + 6 statics
        NotoSerifGeorgian.ttf + manifest + 6 statics
    tests/CodeBrix.Platform.Fonts.Merriweather.Tests/
      CodeBrix.Platform.Fonts.Merriweather.Tests.csproj
      AssemblyMetadataTests.cs
      ContentFilePresenceTests.cs
      ContentManifestTests.cs
      DescriptorTests.cs
      TargetsFileTests.cs
      TestAssetPaths.cs

The `.slnx` carries the two projects plus a "Solution Items" folder
listing AGENT-README.txt, CODEBRIX-DEVELOP.json, icon-codebrix-128.png,
LICENSE, OFL.txt, README.md and THIRD-PARTY-NOTICES.txt, and a
"Solution Items/src" folder holding the buildTransitive `.targets` file.

The `lib/net10.0/CodeBrix.Platform.Fonts.Merriweather/Fonts/` layout
inside the nupkg is load-bearing: the `ms-appx:///` URIs consumers
reference resolve relative to the assembly name, so if the assembly is
ever renamed, the packed content folder must be renamed in lockstep and
every manifest URI plus CODEBRIX-DEVELOP.json must be rewritten.


BUILDING
========================================================================

  dotnet build CodeBrix.Platform.Fonts.Merriweather.slnx

The library csproj sets `GeneratePackageOnBuild=true`, so an ordinary
build also produces a `.nupkg` under
src/CodeBrix.Platform.Fonts.Merriweather/bin/<Configuration>/.

There is no code generation and no native build step. Build time is
dominated by copying the ~39 MB of font binaries into the test project's
output (the test csproj links every `.ttf`, every `.ttf.manifest`, the
`.uprimarker`, the `.targets` file and CODEBRIX-DEVELOP.json into
TestAssets/ with CopyToOutputDirectory="PreserveNewest").


TESTING
========================================================================

  dotnet test CodeBrix.Platform.Fonts.Merriweather.slnx

No opt-in environment variables, no special preparation, no network
access. The suite is pure file/JSON/assembly inspection: xUnit v3 plus
SilverAssertions, with `TestContext.Current.CancellationToken` threaded
through any cancellable call.

What the five test classes pin:

  ContentManifestTests      All four manifests deserialize; entry counts
                            24 / 12 / 6 / 6; the six weights 300-800;
                            Normal+Italic and Normal+SemiCondensed on the
                            primary family; Normal+Italic on Noto Serif;
                            upright-only Armenian and Georgian; every
                            `family_name` rooted at the package's
                            `ms-appx:///...Fonts/` prefix and naming a
                            file that exists; no foreign family token
                            copied in from a sibling package.
  ContentFilePresenceTests  52 `.ttf` total; the 24 Merriweather statics;
                            each companion's variable font, manifest and
                            statics; no `_24pt` optical-size token in any
                            filename; `.uprimarker` present and empty;
                            variable font is a non-trivial size.
  DescriptorTests           CODEBRIX-DEVELOP.json: schemaVersion 1,
                            packageId equal to the published id,
                            displayName "Merriweather", resourceKey
                            "MerriweatherFont", no `#` fragment on the
                            primary or any fallback URI, every URI naming
                            a shipped font, the three companions as the
                            fallback set, no duplicate keyboard layouts,
                            and the presence of the layouts the
                            companions exist to supply.
  TargetsFileTests          The `.targets` exists, declares
                            `CodeBrixRemoveUnusedMerriweather`, hooks
                            `AfterTargets="_CodeBrixAddLibraryAssets"`,
                            uses net10 lib paths, carries the
                            `SupportsFontManifest` condition and never
                            removes a variable font.
  AssemblyMetadataTests     Assembly loads by name, simple name matches,
                            targets .NET 10, exports no public types.

The test project references the library by ProjectReference, so the
tests run against the freshly built assembly, not a restored package.


PACKAGING AND PUBLISHING
========================================================================

Pack driver: `GeneratePackageOnBuild=true` on the library csproj; there
is no separate pack script in this repository.

Versioning: date-stamped and auto-incrementing, computed in the csproj
from `System.DateTime.UtcNow` as 1.<years-since-2026>.<day-of-year>.
<minute-of-day>. Consequences worth remembering: every build yields a new
version; two builds inside the same UTC minute yield the SAME version, so
never publish two packages from within one minute; and the scheme is not
SemVer, so major/minor say nothing about API compatibility. Re-baseline
by changing `_VersionBaseYear`.

What the csproj packs:

  root of the nupkg   icon-codebrix-128.png, README.md, AGENT-README.txt,
                      CODEBRIX-DEVELOP.json, THIRD-PARTY-NOTICES.txt,
                      OFL.txt
  lib/net10.0         the assembly and
                      CodeBrix.Platform.Fonts.Merriweather.uprimarker
  lib/net10.0/CodeBrix.Platform.Fonts.Merriweather/Fonts/
                      every `Fonts/*.ttf` and `Fonts/*.ttf.manifest`
  buildTransitive/    everything under src/.../buildTransitive/**

MAINTAINER-README.txt, EXTRAS-README.txt and README-INDEX.txt are
repository-only files: they are NOT packed. AGENT-README.txt is the file
that ships to consumers, so a consumer-facing correction belongs there.

The `.targets` file name must stay byte-identical to the PackageId
(`CodeBrix.Platform.Fonts.Merriweather.OflLicenseForever.targets`) —
NuGet's auto-import convention matches on that name, and NU5129 warns
when it does not.

Package metadata that must not drift: `PackageLicenseExpression` is
`OFL-1.1`; `PackageRequireLicenseAcceptance` is true; `PackageId` is
`CodeBrix.Platform.Fonts.Merriweather.OflLicenseForever` while
`AssemblyName` / `RootNamespace` / `Product` / `Title` are
`CodeBrix.Platform.Fonts.Merriweather`. DescriptorTests fails if the
descriptor's packageId and the csproj's PackageId disagree.

Ship this package as part of the CodeBrix.Platform family release, not on
its own.


PROVENANCE AND VENDORED SOURCES
========================================================================

Not a port of any upstream packaging project. The csproj, the `.targets`
file, the four `.ttf.manifest` files, CODEBRIX-DEVELOP.json, the
`.uprimarker` and all documentation are original CodeBrix-family files.

The only third-party material is the font binaries, redistributed
bit-for-bit unmodified. Their per-file provenance, the renames applied,
and the SIL OFL 1.1 terms are recorded in THIRD-PARTY-NOTICES.txt
(binary `.ttf` files cannot carry an inline provenance comment).

Font versions as bundled (read from each variable font's `name` table,
recorded here so a refresh can be compared against them):

  Merriweather.ttf ........... Version 2.100
  NotoSerif.ttf .............. Version 2.015
  NotoSerifArmenian.ttf ...... Version 2.008
  NotoSerifGeorgian.ttf ...... Version 2.003

Renames applied on the way in (files only — name tables untouched):

  Merriweather-VariableFont_opsz,wdth,wght.ttf -> Merriweather.ttf
  the `_24pt` optical-size token stripped from every Merriweather static
  each companion's variable font -> dash-free family name

Merriweather declares the Reserved Font Name "Merriweather", so SIL OFL
1.1 condition 3 applies: never alter the font bytes or the internal name
tables. File renames do not create a Modified Version. The three Noto
Serif families declare no Reserved Font Name.

The `keyboardLayouts` array in CODEBRIX-DEVELOP.json is GENERATED, not
hand-written: it is computed by intersecting each software-keyboard
layout's required character set (from the layout definitions in
CodeBrix.Platform) against the `cmap` of every font this package ships,
then taking the union across the primary font and its companions.
Nothing in this repository's build reads CodeBrix.Platform — the array is
computed by a developer-run tool and checked in as data. Regenerate it
whenever the platform's layout set changes or this package's font set
changes.

Regenerating or refreshing the font set — the checklist:

  1. Download from the upstream project; keep the bytes untouched.
  2. Apply the rename conventions above (dash-free name for anything that
     must never be pruned; no optical-size tokens).
  3. Ship only the six weights 300-800 and only the 24pt optical size.
     Adding the other four optical sizes would grow the package by
     roughly 100 MB and the manifest schema could not address them (it
     has no opsz dimension).
  4. Rewrite the affected `.ttf.manifest` file(s); entry counts are
     asserted by tests, so update the tests deliberately, never to make
     a red test pass.
  5. Re-run the keyboardLayouts generator and update THIRD-PARTY-
     NOTICES.txt with new versions, files and renames.
  6. Update AGENT-README.txt (inventory, coverage counts, quick
     reference card) in the same change.

The prune target's mechanics, for anyone editing the `.targets` file:
it removes `_AllChildProjectItemsWithTargetPath` entries matching
`...\lib\net10.0\CodeBrix.Platform.Fonts.Merriweather\Fonts\**-**.ttf`
when `'$(SupportsFontManifest)'!='true'`. The match is on the DASH in the
file name — that is the whole mechanism, and it is why every font that
must survive the prune is named without a dash. Adding a dash-free static
instance would silently make it un-prunable; adding a dash to a variable
font would silently delete a script.


CODING CONVENTIONS
========================================================================

Standard CodeBrix family conventions apply; the ones that bite here:

  * Target framework: net10.0 only. No multi-targeting.
  * Nullable reference types: OFF (do not set `<Nullable>enable</Nullable>`).
    No `?` annotations on reference types, no `!` null-forgiveness.
    Value-type nullables are fine.
  * No global usings.
  * `<GenerateDocumentationFile>true</GenerateDocumentationFile>` is on;
    every public member of a public type needs an XML doc comment and
    CS1591 is fixed at source, never suppressed. The library currently
    has no public types, so this is trivially satisfied — it stops being
    trivial the moment anyone adds one.
  * No project-level warning suppression (`<NoWarn>`, `<WarningLevel>0`,
    `<TreatWarningsAsErrors>false</>` and friends are forbidden).
  * Tests: xUnit v3 + SilverAssertions, one `<Class>Tests.cs` per subject,
    snake_case test method names, //Arrange //Act //Assert comment
    blocks, `TestContext.Current.CancellationToken` on cancellable calls.
  * Every packaging library ships an InternalsVisibleTo.cs granting its
    `.Tests` assembly access.
  * The whole package — wrapper and fonts — is SIL OFL 1.1.

For the full list of family conventions see
CODEBRIX_LIBRARY_OBSERVATIONS.txt in the CodeBrix.Library.Dev-private
repository.


NOTES
========================================================================

  * The `keyboardLayouts` array claims `el`, `ka` and `hy`, which are
    delivered by the companion fonts and therefore depend on
    CodeBrix.Platform consulting `fallbackFontUris` when the primary font
    lacks a glyph. That claim was published deliberately, with the
    platform work following immediately; if the fallback support is ever
    removed, the descriptor must be revisited.
  * Coverage figures quoted in AGENT-README.txt were measured from the
    bundled variable fonts' `cmap` tables (1,423 / 2,965 / 430 / 509
    codepoints). Re-measure after any font refresh.
  * `bin/` and `obj/` folders in this working tree may contain stale
    `.nupkg` files from earlier builds; they are not authoritative.
