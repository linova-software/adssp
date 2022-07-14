// ADSSP - Active Directory Self-Service Portal
// Copyright (c) 2022  Linova Software GmbH
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Runtime.Serialization;
using System.Threading.Tasks;
using LanguageExt;
using LanguageExt.Common;
using Linova.ActiveDirectory.SelfService.Configuration.CommSpec;
using Linova.ActiveDirectory.SelfService.Domain.ChangePassword.CommSpec;
using Linova.ActiveDirectory.SelfService.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Linova.ActiveDirectory.SelfService.Pages;

[IgnoreAntiforgeryToken]
public class ChangePasswordPageModel : PageModel
{
    public class SuccessMessage : NewType<SuccessMessage, string>
    {
        public SuccessMessage(string value) : base(value)
        {
        }

        public SuccessMessage(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    private readonly IMediator _mediator;

    [BindProperty]
    public ChangePasswordModel PasswordChange { get; set; } = new();

    public Either<Seq<Error>, SuccessMessage> Message { get; private set; }

    public Task<string> DomainName { get; }

    public ChangePasswordPageModel(IMediator mediator)
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
        if (!TryValidateModel(PasswordChange, nameof(PasswordChange)))
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

        var commandResponse = await _mediator.Send(new ChangePasswordCommand(
            await DomainName,
            PasswordChange.Username!,
            PasswordChange.Password!,
            PasswordChange.NewPassword!));

        Message = commandResponse.Match<Either<Seq<Error>, SuccessMessage>>(
            _ => new SuccessMessage(Messages.DirectoryServices.PasswordChangeSuccess),
            error => Seq<Error>.Empty.Add(error));

        return Page();
    }
}
