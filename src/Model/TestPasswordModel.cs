// ADSSP - Active Directory Self-Service Portal
// Copyright (c) 2022  Linova Software GmbH
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Linova.ActiveDirectory.SelfService.Model;

public class TestPasswordModel : IValidatableObject
{
    [Required]
    public string? Username { get; set; }

    [Required]
    public string? Password { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var nullFields = new List<string>(4);
        if (Username == null)
        {
            nullFields.Add(nameof(Username));
        }

        if (Password == null)
        {
            nullFields.Add(nameof(Password));
        }

        if (nullFields.Count > 0)
        {
            yield return new ValidationResult(Messages.Validation.FieldCannotBeNull, nullFields);
        }
    }
}
