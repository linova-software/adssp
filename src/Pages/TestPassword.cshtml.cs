// ADSSP - Active Directory Self-Service Portal
// Copyright (c) 2022  Linova Software GmbH
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Threading.Tasks;
using LanguageExt;
using LanguageExt.Common;
using Linova.ActiveDirectory.SelfService.Configuration.CommSpec;
using Linova.ActiveDirectory.SelfService.Domain.TestPassword.CommSpec;
using Linova.ActiveDirectory.SelfService.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Linova.ActiveDirectory.SelfService.Pages;

[IgnoreAntiforgeryToken]
public class TestPasswordPageModel : PageModel
{
    private readonly IMediator _mediator;

    [BindProperty]
    public TestPasswordModel FormModel { get; set; } = new();

    public Either<Error, bool> TestResult { get; private set; }

    public Task<string> DomainName { get; }

    public TestPasswordPageModel(IMediator mediator, ILogger<TestPasswordPageModel> logger)
    {
        _mediator = mediator;

        DomainName = GetDomainNameAsync();
    }

    private async Task<string> GetDomainNameAsync()
    {
        return (await _mediator.Send(new ActiveDirectoryQueryOptionsRequest()).ConfigureAwait(false)).DomainName;
    }

    public IActionResult OnPostTryValidate()
    {
        if (!TryValidateModel(FormModel, nameof(FormModel)))
        {
            return Page();
        }

        return RedirectToPage(PageNames.Home);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        TestResult = await _mediator.Send(new TestPasswordRequest(
            await DomainName,
            FormModel.Username,
            FormModel.Password));

        return Page();
    }
}
