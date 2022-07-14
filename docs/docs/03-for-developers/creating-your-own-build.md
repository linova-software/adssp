---
sidebar_position: 2
title: Building a production version
---

Open a command prompt and navigate to the `src/` directory of this repository.

If you want to specify a different software version than the version as released to GitHub (let's assume you want `X.Y.Z`), go to the `src/Linova.ActiveDirectory.SelfService.csproj` project file and edit the following two properties:

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    ...
    <AssemblyVersion>X.Y.Z</AssemblyVersion>
    <FileVersion>X.Y.Z</FileVersion>
    ...
  </PropertyGroup>
  ...

</Project>
```

Execute `dotnet publish -c Release -r win-x64 --no-self-contained -o bin/publish`. This will create a release version of the software in the folder `src/bin/publish` (relative to the git repository root).

If you prefer a version with bundled runtime, just replace `--no-self-contained` with `--self-contained` in the above command.

:::caution
Be aware that bundling the runtime requires you to re-build the software every time you want to update the runtime.
:::
