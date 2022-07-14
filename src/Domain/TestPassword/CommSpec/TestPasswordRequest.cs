// ADSSP - Active Directory Self-Service Portal
// Copyright (c) 2022  Linova Software GmbH
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using LanguageExt;
using LanguageExt.Common;
using MediatR;

namespace Linova.ActiveDirectory.SelfService.Domain.TestPassword.CommSpec;

public record TestPasswordRequest(string DomainName, string? Username, string? Password) : IRequest<Either<Error, bool>>;
