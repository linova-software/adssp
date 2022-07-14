---
sidebar_position: 3
title: ADSSP on Unix
---

:::warning
Currently, **we do not support** running the software on Unix systems and strongy advise against it.
:::

## Why we do not support the software on Unix

There are several reasons for this decision:

- We have **not tested** it, and we do not plan to have a proper setup for testing it. Since the software changes things in a very central part of many companies' IT infrastructure - authentication - we consider it safety-critical and thus will not provide you with an official version which has not been tested.
- We could not identify a use case where anyone operates an Active Directory, but cannot deploy this software on a Windows host. Although you may have the case that you operate only a single Windows host in your entire network - the one running the Active Directory - and otherwise only Unix systems, you can still **run this software on the Windows Server with the Active Directory** on it.
- Although Unix hosts can join an Active Directory as a computer, there can easily be a **collision** that renders the otherwise working setup dysfunctional: If you install **OpenSSH on a domain controller**, then it exposes the service user for OpenSSH to the Active Directory. If you also install **openssh-server** on e.g. a **Linux** host, then there also - by default - exists a user openssh. Resolving this conflict can be quite challenging - and in the meantime, the **ADSSP will not work** the same way as if it was installed on a Windows host (there, the local user and the directory user for OpenSSH are separated correctly).

## How to run the software on Unix anyway

As a starting point, you will need the same setup as described in [Getting Started](./getting-started.md).

Adapt the `TargetFramework` in the `Linova.ActiveDirectory.SelfService.csproj` file to be `net6.0` instead of `net6.0-windows`. Also, remove the `switch(Environment.OSVersion.Platform) { ... }` statement at the top of `Program.cs`. This will disable the check for the platform.

You should remove the reference to the NuGet package [Microsoft.Extensions.Hosting.WindowsServices](https://www.nuget.org/packages/Microsoft.Extensions.Hosting.WindowsServices/). You must then also either remove the line with `builder.Host.UseWindowsService()` in `Program.cs` or replace it with an appropriate alternative (e.g. using the `UseSystemd()` extension method from the [corresponding Systemd package](https://www.nuget.org/packages/Microsoft.Extensions.Hosting.Systemd/)).

Make sure that all other libraries included via NuGet packages are also available for your platform. You will probably get a warning by NuGet or the compiler if they are not, but in case something does not work this might be something to look into.

:::caution
We use the .NET 6 platform extensions for the change password functionality. These extensions are only available on Windows, so you will get a `PlattformNotSupportedException` thrown by Microsoft's library if you try to use this feature on Unix systems.

If you want to use this functionality, you need to replace it by another library.
:::

If you then want to create a production build, see [Building a production version](./creating-your-own-build.md). Of course, you should change the appropriate runtime identifier in the `dotnet publish` command (as opposed to `win-x64`).
