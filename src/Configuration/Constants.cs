// ADSSP - Active Directory Self-Service Portal
// Copyright (c) 2022  Linova Software GmbH
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;

namespace Linova.ActiveDirectory.SelfService.Configuration;

public static class Constants
{
    public const string NoServiceCommandLineFlag = "--NO-SERVICE";
    public const string EnvironmentVariablesPrefix = "ADSSP_";

    public const string LogFileNamePrefix = "adssp-";
    public const string LogFileExtension = "log";
    public const string LogDefaultDirectory = "logs";
    public const string LogDirectoryConfigKey = "Logging:LogDirectory";
    public const int LogRetention = 28;
    public static readonly TimeSpan LogFlushPeriod = TimeSpan.FromSeconds(2);

    public const string QueryOptionsConfigKey = "Portal";

    public const string RateLimitingConfigKey = "ClientRateLimiting";
    public const string RateLimitingPoliciesConfigKey = "ClientRateLimitPolicies";
}
