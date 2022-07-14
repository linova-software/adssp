---
sidebar_position: 1
title: Getting Started
---

## Requirements

- .NET 6.0 SDK
- Node.js 16 (if you want to build the docs as well)

## Coding Style

We adhere mostly to the [.NET Runtime Coding Guidelines](https://github.com/dotnet/runtime/blob/v6.0.6/docs/coding-guidelines/coding-style.md).

Thus, we also supplied an EditorConfig file ([`.editorconfig`](https://github.com/linova-software/adssp/blob/master/src/.editorconfig)) in the projects root directory, which will help your IDE adhere to the coding style.

Also, the compiler will use these information (and additionally the [`CodeAnalysis.src.globalconfig`](https://github.com/linova-software/adssp/blob/master/src/CodeAnalysis.src.globalconfig)) to check the coding style when compiling.

## Running The Project

Either use your IDE to start the project, or just use `dotnet run` on the command line. You may append `--launch-profile "IIS Express"` or `--launch-profile Kestrel` to run one either a pre-configured IIS Express or Kestrel server instance for testing locally.

For local testing with hot-reload capabilities, you can use `dotnet watch run -- --launch-profile Kestrel` instead.

## Further Reading

For more information, have a look at the official [Microsoft documentation](https://docs.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/?view=aspnetcore-6.0) for ASP.NET Core 6.0 and Razor Pages.
