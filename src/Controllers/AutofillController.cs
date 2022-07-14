// ADSSP - Active Directory Self-Service Portal
// Copyright (c) 2022  Linova Software GmbH
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Threading;
using System.Threading.Tasks;
using Linova.ActiveDirectory.SelfService.Configuration.CommSpec;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Linova.ActiveDirectory.SelfService.Controllers;

[Authorize]
[Route("/password/autofill")]
public class AutofillController : Controller
{
    private readonly IMediator _mediator;

    public AutofillController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var adQueryOptions = await _mediator.Send(new ActiveDirectoryQueryOptionsRequest(), cancellationToken).ConfigureAwait(true);

        var usernameDirectoryPrefixLength = adQueryOptions.DomainName.Length + 1;

        if (User.Identity is not { IsAuthenticated: true }
            || User.Identity.Name == null
            || !User.Identity.Name.StartsWith(adQueryOptions.DomainName + "\\", StringComparison.InvariantCulture))
        {
            return RedirectToPage(PageNames.ChangePassword);
        }

        return RedirectToPage(PageNames.ChangePassword, new
        {
            accountName = User.Identity.Name[usernameDirectoryPrefixLength..]
        });
    }
}
