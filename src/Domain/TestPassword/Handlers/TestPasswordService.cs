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
using Linova.ActiveDirectory.SelfService.Domain.TestPassword.CommSpec;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Linova.ActiveDirectory.SelfService.Domain.TestPassword.Handlers;

public class TestPasswordService : RequestHandler<TestPasswordRequest, Either<Error, bool>>
{
    private readonly ILogger _logger;

    public TestPasswordService(ILogger<TestPasswordService> logger)
    {
        _logger = logger;
    }

    protected override Either<Error, bool> Handle(TestPasswordRequest request)
    {
        try
        {
            using var pc = new PrincipalContext(ContextType.Domain, request.DomainName, request.Username, request.Password);

            var isPasswordCorrect = pc.ValidateCredentials(request.Username, request.Password);

            return isPasswordCorrect;
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

            return Error.New(ex.Message);
        }
    }
}
