# Changelog

## Version [5.2.0](https://github.com/CedX/FreeMobile.net/compare/v5.1.0...v5.2.0)
- Added the `Client.UserAgent` property.

## Version [5.1.0](https://github.com/CedX/FreeMobile.net/compare/v5.0.0...v5.1.0)
- Added the `cancellationToken` parameter to the `Client.SendMessage()` method.

## Version [5.0.0](https://github.com/CedX/FreeMobile.net/compare/v4.0.0...v5.0.0)
- Breaking change: raised the required target framework.
- Ported the [C#](https://learn.microsoft.com/en-us/dotnet/csharp) cmdlets to [PowerShell](https://learn.microsoft.com/en-us/powershell).

## Version [4.0.0](https://github.com/CedX/FreeMobile.net/compare/v3.3.0...v4.0.0)
- Breaking change: renamed the asynchronous `Client.SendMessage()` method to `SendMessageAsync()`.
- Added the synchronous `Client.SendMessage()` method.

## Version [3.3.0](https://github.com/CedX/FreeMobile.net/compare/v3.2.0...v3.3.0)
- Merged the [C#](https://learn.microsoft.com/en-us/dotnet/csharp) and [PowerShell](https://learn.microsoft.com/en-us/powershell) projects into one solution.

## Version [3.2.0](https://github.com/CedX/FreeMobile.net/compare/v3.1.0...v3.2.0)
- Restored support for [.NET](https://dotnet.microsoft.com/en-us) 9.

## Version [3.1.0](https://github.com/CedX/FreeMobile.net/compare/v3.0.0...v3.1.0)
- Added constructor overloads to the `Client` class.

## Version [3.0.0](https://github.com/CedX/FreeMobile.net/compare/v2.0.0...v3.0.0)
- Added support for [.NET](https://dotnet.microsoft.com/en-us) 10.

## Version [2.0.0](https://github.com/CedX/FreeMobile.net/compare/v1.0.1...v2.0.0)
- Breaking change: removed the `Account` and `ApiKey` properties from the `Client` class.
- Added a constructor that accepts a `System.Net.NetworkCredential` object for the Free Mobile user name and password.

## Version [1.0.1](https://github.com/CedX/FreeMobile.net/compare/v1.0.0...v1.0.1)
- Included the debug symbols in the package published on [NuGet](https://www.nuget.org).

## Version 1.0.0
- Initial release.
