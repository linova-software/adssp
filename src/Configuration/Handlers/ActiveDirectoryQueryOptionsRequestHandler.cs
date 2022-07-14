// ADSSP - Active Directory Self-Service Portal
// Copyright (c) 2022  Linova Software GmbH
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Linova.ActiveDirectory.SelfService.Configuration.CommSpec;
using MediatR;
using Microsoft.Extensions.Options;

namespace Linova.ActiveDirectory.SelfService.Configuration.Handlers;

public class ActiveDirectoryQueryOptionsRequestHandler : RequestHandler<ActiveDirectoryQueryOptionsRequest, ActiveDirectoryQueryOptions>
{
    private readonly IOptions<ActiveDirectoryQueryOptions> _options;

    public ActiveDirectoryQueryOptionsRequestHandler(IOptions<ActiveDirectoryQueryOptions> options)
    {
        _options = options;
    }

    protected override ActiveDirectoryQueryOptions Handle(ActiveDirectoryQueryOptionsRequest request)
    {
        return _options.Value;
    }
}
