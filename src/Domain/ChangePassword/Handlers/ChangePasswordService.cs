// ADSSP - Active Directory Self-Service Portal
// Copyright (c) 2022  Linova Software GmbH
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.DirectoryServices.AccountManagement;
using LanguageExt;
using LanguageExt.Common;
using Linova.ActiveDirectory.SelfService.Domain.ChangePassword.CommSpec;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Linova.ActiveDirectory.SelfService.Domain.ChangePassword.Handlers;

public class ChangePasswordService : RequestHandler<ChangePasswordCommand, Either<Error, Success>>
{
    private readonly ILogger _logger;

    public ChangePasswordService(ILogger<ChangePasswordService> logger)
    {
        _logger = logger;
    }

    protected override Either<Error, Success> Handle(ChangePasswordCommand request)
    {
        try
        {
            using var pc = new PrincipalContext(ContextType.Domain, request.DomainName, request.Username, request.Password);

            using var user = UserPrincipal.FindByIdentity(pc, IdentityType.SamAccountName, request.Username);

            if (user == null)
            {
                return Error.New(Messages.DirectoryServices.UserNotFound);
            }

            if (user.UserCannotChangePassword)
            {
                return Error.New(Messages.DirectoryServices.UserCannotChange);
            }

            user.ChangePassword(request.Password, request.NewPassword);

            return default(Success);
        }
        catch (PasswordException ex)
        {
            return Error.New(ex);
        }
        catch (Exception ex) when (ex.HResult == HResults.UsernamePasswordWrong)
        {
            return Error.New(Messages.DirectoryServices.UnknownUserOrIncorrectPassword);
        }
        catch (Exception ex)
        {
            Messages.UnexpectedError(_logger, ex);

            return Error.New(ex);
        }
    }
}
