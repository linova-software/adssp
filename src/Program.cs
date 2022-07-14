// ADSSP - Active Directory Self-Service Portal
// Copyright (c) 2022  Linova Software GmbH
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspNetCoreRateLimit;
using Linova.ActiveDirectory.SelfService;
using Linova.ActiveDirectory.SelfService.Configuration;
using MediatR;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetEscapades.Extensions.Logging.RollingFile;

switch (Environment.OSVersion.Platform)
{
    case PlatformID.Win32NT:
        break;
    case PlatformID.Win32S:
    case PlatformID.Win32Windows:
    case PlatformID.WinCE:
    case PlatformID.Xbox:
        await Console.Error.WriteLineAsync(Messages.Platform.Deprecated).ConfigureAwait(false);
        return HResults.DeprecatedPlatform;
    case PlatformID.MacOSX:
    case PlatformID.Unix:
        await Console.Error.WriteLineAsync(Messages.Platform.UnixNotSupported).ConfigureAwait(false);
        return HResults.UnixPlatform;
    case PlatformID.Other:
    default:
        await Console.Error.WriteLineAsync(Messages.Platform.Unknown).ConfigureAwait(false);
        return HResults.UnknownPlatform;
}

var hostRunsAsService = true;
if (args.Length > 0 && args[0].ToUpperInvariant() == Constants.NoServiceCommandLineFlag)
{
    hostRunsAsService = false;
    args = args.Skip(1).ToArray();
}

var builder = WebApplication.CreateBuilder(args);
if (hostRunsAsService)
{
    builder.Host.UseWindowsService();
}

// Configure host to use only HTTP/1 because HTTP/2 and 3 are not supported for the Negotiate authentication protocol
// and we do not want to waste time first having a HTTP/2 connection then forcing a downgrade
builder.Services.Configure<ListenOptions>(options => options.Protocols = HttpProtocols.Http1);

// !! IMPORTANT !!
// We do not have to configure IIS here because it automatically handles a potential downgrade if this app is run in IIS.
// For more information, see: https://docs.microsoft.com/en-us/iis/get-started/whats-new-in-iis-10/http2-on-iis#when-is-http2-not-supported

// !! IMPORTANT !!
// We CANNOT configure HTTP.sys to not use HTTP/2 because there is not configuration option at this time (on the kernel
// level); the application will downgrade automatically to HTTP/1 (as kestrel would)
// For more information, see: https://docs.microsoft.com/en-us/iis/get-started/whats-new-in-iis-10/http2-on-iis#when-is-http2-not-supported

builder.Configuration.AddEnvironmentVariables(Constants.EnvironmentVariablesPrefix);

builder.Logging.AddConsole();
builder.Logging.AddFile(options =>
{
    options.FileName = Constants.LogFileNamePrefix;
    options.LogDirectory = builder.Configuration[Constants.LogDirectoryConfigKey] ?? Path.Join(Environment.CurrentDirectory, Constants.LogDefaultDirectory);
    options.Periodicity = PeriodicityOptions.Daily;
    options.Extension = Constants.LogFileExtension;
    options.FlushPeriod = Constants.LogFlushPeriod;
    options.RetainedFileCountLimit = Constants.LogRetention;
});

builder.Services.AddMemoryCache();
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddOptions<ClientRateLimitOptions>()
    .Configure(options =>
    {
        options.EnableEndpointRateLimiting = true;
        options.QuotaExceededMessage = "";
        options.DisableRateLimitHeaders = true;
        options.GeneralRules = new List<RateLimitRule>
        {
            new()
            {
                Endpoint = "post:*", Limit = 1, PeriodTimespan = TimeSpan.FromSeconds(2)
            }
        };
    })
    .BindConfiguration(Constants.RateLimitingConfigKey);
builder.Services.AddOptions<ClientRateLimitPolicies>()
    .BindConfiguration(Constants.RateLimitingPoliciesConfigKey);
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddOptions<ActiveDirectoryQueryOptions>()
    .BindConfiguration(Constants.QueryOptionsConfigKey)
    .Validate(options => !string.IsNullOrEmpty(options.DomainName), Messages.Configuration.DomainNameEmpty)
    .Validate(options => options.DomainName != null && options.DomainName == options.DomainName.ToUpperInvariant(), Messages.Configuration.DomainNameNotUppercase)
    .Validate(options => options.DomainName != null && options.DomainName.Trim() == options.DomainName, Messages.Configuration.DomainNameWhitespaces)
    .ValidateOnStart();
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
    .AddNegotiate(n =>
    {
        n.Events = new NegotiateEvents
        {
            OnChallenge = ctx =>
            {
                ctx.Response.OnStarting(async () =>
                {
                    await ctx.Response.Body.WriteAsync(Messages.AuthFailureHtmlBytes);
                });
                return Task.CompletedTask;
            },
            OnAuthenticated = ctx =>
            {
                if (ctx.Principal?.Identity == null || !ctx.Principal.Identity.IsAuthenticated || ctx.Principal.Identity.Name == null)
                {
                    return Task.CompletedTask;
                }

                var nameSplit = ctx.Principal.Identity.Name.Split('\\', 2);
                if (nameSplit.Length != 2)
                {
                    ctx.Fail(Messages.DirectoryServices.WrongUsernameFormat(ctx.Principal.Identity.Name));
                }

                var adSettings = ctx.HttpContext.RequestServices.GetRequiredService<IOptions<ActiveDirectoryQueryOptions>>().Value;

                if (nameSplit[0].ToUpperInvariant() != adSettings.DomainName)
                {
                    ctx.Fail(Messages.DirectoryServices.UnsupportedDomain(nameSplit[0]));
                }

                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

builder.Services
    .AddRazorPages(p => { p.Conventions.AuthorizePage(PageNames.AuthTest); })
    .AddMvcOptions(o =>
    {
        o.Filters.Add<IgnoreAntiforgeryTokenAttribute>();
        o.MaxModelValidationErrors = 0;
        o.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(_ => Messages.Validation.FieldRequired);
    })
    .AddViewOptions(o => { o.HtmlHelperOptions.ClientValidationEnabled = false; });
builder.Services.AddMvc();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler(PageNames.Error);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseClientRateLimiting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllers();
    endpoints.MapDefaultControllerRoute();
});

await app.RunAsync().ConfigureAwait(false);
return 0;
