// ADSSP - Active Directory Self-Service Portal
// Copyright (c) 2022  Linova Software GmbH
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Linova.ActiveDirectory.SelfService;

public static class HResults
{
    public const int UsernamePasswordWrong = unchecked((int)0x8007052E);

    public const int DeprecatedPlatform = unchecked((int)0xA000EEE1);
    public const int UnixPlatform = unchecked((int)0xA000EEE2);
    public const int UnknownPlatform = unchecked((int)0xA000EEEE);
}
