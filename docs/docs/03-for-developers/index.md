---
title: For Developers
---

This section is targeted specifically for developers or administrators who want to get more involved than just deploying the default, pre-built version of the Active Directory Self-Service Portal.

## Contributing

Whether you have made fancy customizations to the software, fixed a bug or developed a new feature: We love to see awesome contributions to open source projects like this.

If you want to contribute a feature or fix a bug, please create a GitHub issue **before** you start your work on the code. This helps us better coordinate contributions and avoid duplicate work.

As soon as we agreed on a concrete realization, you can start working on the code and open a pull request to merge your changes into the upstream. Make sure to include the issue number in the pull request using a `Implements #1111` in the pull request's description (given that 1111 is the issue number).

Your code contributions must adhere to the code style as described in [Getting Started](./getting-started.md#coding-style).

## Error Handling

We do not throw exceptions across component boundaries.

If errors need to be transported over the boundaries, the called component's function shall return an object of type `Either<Error, R>` where `R` is the return type for successful execution. Both `Either` and `Error` are types of the `LanguageExt.Core` library.
