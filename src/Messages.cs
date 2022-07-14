// ADSSP - Active Directory Self-Service Portal
// Copyright (c) 2022  Linova Software GmbH
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Text;
using LanguageExt;
using Microsoft.Extensions.Logging;

namespace Linova.ActiveDirectory.SelfService;

internal static class Messages
{
    public static readonly Action<ILogger, Exception?> UnexpectedError = LoggerMessage.Define(LogLevel.Error, 0, "An unexpected error occurred");

    public const string AuthFailureHtml =
        @"<!DOCTYPE html><html><head><meta charset='utf-8'><title>Authentication Failed - Active Directory Self-Service Portal</title><meta name='viewport' content='width=device-width, initial-scale=1'></head><body style=""position:fixed;top:50%;left:50%;font-size:18pt;font-family:sans-serif;""><div style=""position:relative;top:-50%;left:-50%;text-align:center;""><div>If you see this message, you either failed authentication or your browser does not support the authentication protocol.</div><div style=""margin-top:2em;""><a href=""../"">Back to home page</a></div></div></body></html>";

    public static readonly ReadOnlyMemory<byte> AuthFailureHtmlBytes = Encoding.UTF8.GetBytes(AuthFailureHtml);

    public static class Configuration
    {
        public const string DomainNameEmpty = "Specify the name of your domain";
        public const string DomainNameNotUppercase = "Specify the name of you domain in uppercase only";
        public const string DomainNameWhitespaces = "Please remove leading and trailing whitespaces from your domain name";
    }

    public static class Platform
    {
        public const string Deprecated = "Deprecated platform, please run on Windows NT or later";
        public const string UnixNotSupported = "Unix systems are not supported. If you want to run it anyway see the docs under 'Develop > ADSSP on Unix'.";
        public const string Unknown = "Unknown platform, please run on Windows";
    }

    public static class Validation
    {
        public const string FieldRequired = "The field is required.";
        public const string FieldCannotBeNull = "Field cannot be null";
    }

    public static class DirectoryServices
    {
        public const string UserCannotChange = "User cannot change password";
        public const string UserNotFound = "User not found in directory";
        public const string UnknownUserOrIncorrectPassword = "Unknown user or incorrect password";
        public const string PasswordsDifferent = "Passwords do not match";
        public const string PasswordChangeSuccess = "Password changed!";
        public static readonly Func<string, string> UnsupportedDomain = Prelude.memo((string domain) => $"Domain not supported: {domain}");
        public static readonly Func<string, string> WrongUsernameFormat = Prelude.memo((string userName) => $"Wrong format: Expected DOMAIN\\USER, got {userName}");
    }
}
