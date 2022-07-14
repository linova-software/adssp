---
sidebar_position: 3
title: Security Considerations
---

As with any software, we can only list a few application-specific considerations. You still have to decide for yourself which of these considerations apply to your environment and what other considerations you have to make to deploy the application securely.

## Secrets

As the application itself does not operate under its own user, it just forwards the users' inputs, there are no secrets to be stored and no impersonation occurs.

## Exposing the Application to the Internet

We see the **default use case** for deplyoing this application in your **intranet**, because usually the Active Directory is also not exposed to the internet.

Apart from any **potential vulnerabilities** in the Kerberos or NTLM protocols, which may be a security threat if you expose the application to the internet, attackers could use the portal to **brute force** username/password combinations.

:::caution
Thus, we **cannot recommend** exposing the application to the outside world.
:::

## Denial of Service

This applies if you have configured your Domain Group Policies such that account lockout is activated - i.e. the Active Directory locks a user's account if there are too many failed login attempts.

The Active Directory Self-Service Portal performs an operation that is recognized by the domain controller(s) as a login attempt, and thus it might lead to a user being locked out of their account (= denial of service). As any other login method against an Active Directory, it can also be abused to deny a specific user to log in.

However, the ADSSP has a built-in feature to mitigate brute-force attacks.
