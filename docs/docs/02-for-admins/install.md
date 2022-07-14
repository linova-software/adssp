---
sidebar_position: 1
title: Installation
---

This page will explain how to install the Active Directory Self-Service Portal. After installation, you should also [configure](./configuration.md) the application.

## Download

Download the desired release ZIP file from [GitHub](https://github.com/linova-software/adssp/releases). You can find the latest release [here](https://github.com/linova-software/adssp/releases/latest).

You can choose between

- a version without included runtime (**recommended**):
  - The file name looks like this: `adssp-v1.0.0.zip`.
  - The download is very small for every ADSSP release (approx. 10 MB).
  - You need to install the [ASP.NET Core Hosting Bundle](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) separately.
  - The hosting bundle also adds support for **IIS**.
  - Can update automatically through Windows Update, without having to reinstall the ADSSP.
- a version with included runtime:
  - The file name looks like this: `adssp-v1.0.0-with-runtime.zip`.
  - The download is quite large for every ADSSP release (a few hundred MB).
  - You do not need to install anything else.
  - IIS is not supported, only the built-in Kestrel server.
  - You have to download and reinstall the package for every new release of the ADSSP.

## Installation

Unpack the downloaded ZIP file to an arbitrary location on the target system. Choosing a location without any spaces in the path simplifies installing it as a service.

### Installing as a Windows Service

You can install the ADSSP as a Windows Service, so that it automatically starts.

You can change all of the following parameters, but we will assume the following:

- The name (unique identifier, used e.g. in scripts) is `adssp`.
- The display name is shown in the service overview or when querying services, set to `Active Directory Self-Service Portal`.
- You unpacked the downloaded ZIP file to `C:\adssp`.
- You want the ADSSP to automatically start when the computer it is installed on starts, thus we set the startup type to `Automatic`.
- You want to run the software using a managed or virtual service account.

Using Powershell:

```powershell
New-Service -Name "adssp" -DisplayName "Active Directory Self-Service Portal" -BinaryPathName "C:\adssp\Linova.ActiveDirectory.SelfService.exe" -StartupType Automatic
```

Using [NSSM](https://nssm.cc): Also includes a GUI for creating and editing the service.

You may want to use a separate, restricted service account (best: a Group-managed Service Account) under which the service runs.
