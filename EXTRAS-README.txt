========================================================================
EXTRAS-README: CodeBrix.Platform.Fonts.Merriweather
Samples, tools and other content in this repository that is not part of
a NuGet package
========================================================================

This repository contains NO sample applications, demo apps, tools,
scripts or optional test-data downloads. It is a single font asset
package plus the test project that guards it.

The only non-package content is the test project:

  tests/CodeBrix.Platform.Fonts.Merriweather.Tests/
      xUnit v3 + SilverAssertions test project. It is not packed and is
      not published; it exists to pin the package's contents (font file
      set, manifest entries, descriptor fields, `.targets` behaviour and
      assembly metadata). Run it with:

          dotnet test CodeBrix.Platform.Fonts.Merriweather.slnx

      No opt-in environment variables, no downloads, no special prep.
      See MAINTAINER-README.txt for what each test class asserts.

Two things that might look like repository content but are not:

  * The tool that generates the `keyboardLayouts` array in
    CODEBRIX-DEVELOP.json is developer-run and lives outside this
    repository; only its OUTPUT is checked in, as data.
  * `bin/` and `obj/` folders may hold `.nupkg` and build artefacts from
    earlier local builds. They are build output, not repository content.
