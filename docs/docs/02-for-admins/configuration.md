---
sidebar_position: 2
title: Configuration
---

On this page you will find an explanation for all configuration options the application exposes and some links for further reading.

## Kerberos

If you want Kerberos authentication to work as well, you will need to set a Service Principal Name (SPN) for the account under which the ADSSP runs. The default will be the computer account (if installed as a service) or the current user (if you execute interactively).

See the [Microsoft documentation](https://docs.microsoft.com/en-us/windows-server/networking/sdn/security/kerberos-with-spn) for information on how to configure this.

## TLS

The Active Directory Self-Service portal supports TLS up to TLS 1.3, HSTS and HTTP-to-HTTPS redirection if you use the built-in Kestrel web server. See [Configure Endpoints](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel/endpoints?view=aspnetcore-6.0) in the official Microsoft Documentation for information on how to specify the server's SSL certificate.

If you use any other web server, like IIS or HTTP.sys, please refer to their corresponding documentation for more information.

## Logging

For logging, this software relies on the ASP.NET Core integrated solution for logging provided by Microsoft.

### Configuring which log levels are printed

In general, the producedure to configure logging and the log levels printed for each category is described in the [corresponding Microsoft documentation](https://docs.microsoft.com/en-us/dotnet/core/extensions/logging#configure-logging). You can also provide provider-specific configuration.

We cannot list all possible log categories, but all logs messages that are coming from the ADSSP itself come reside under the `Linova.ActiveDirectory.SelfService` category. If you want to configure messages which are output by a specific class, see for all constructor-injected `ILogger<NameOfTheClass>` loggers; the classes' namespace plus the name of the class (which the logger is injected to) is the logging category which you can configure.

### Logging Providers

This application uses five logging providers (also sometimes called log sinks).

The first four are included by default in ASP.NET Core applications and described in the [Microsoft Docs](https://docs.microsoft.com/en-us/dotnet/core/extensions/logging-providers).

The other provider is a rolling file provider: This means that log messages are printed to a file; files are retained for 28 days, then files older than 28 days are deleted automatically.

### Settings for File Logging

:::caution
We **strongly recommend** you to configure a logging directory that is not the current working directory.
:::

You can configure the logging directory where the rolling file logs are placed using the configuration key `Logging:LogDirectory`. You can use both forward and backward slashes on Windows and there is no rule that the path must end or not end with a slash. If you do not configure the logging directory, it will fall back to the folder `logs` in the current working directory.

Other, currently not configurable parameters are:

- Prefix: The file name will always start with `adssp-`
- Extension: The file extension will always be `.log`
- Periodicity: The log files will rolled over daily
- Flush period: The file will be flushed every 2 seconds
- Retention: 28 log files will be retained
