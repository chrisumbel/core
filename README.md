# SteelToe OSS Core
SteelToe is a collection of libraries designed to facilitate the consumption of several OSS packages such as **Spring Cloud Configuration Server**, **Eureka** (service discovery and registration), as well as provide .NET implementations for some Netflix OSS components like **Circuit Breakers**.

In this repository, you'll find 3 projects:

# SteelToe.Core
This is the main library. Reference this package to gain access to the building blocks for SteelToe, including code that will seamlessly inject **Cloud Foundry** bound service details into your application.

# SteelToe.Core.Test
Unit and integration tests for the **SteelToe.Core** library.

# SteelToe.Core.Sample
A sample service written in ASP.NET 5 to illustrate the recommended best practice for enabling cloud foundry bound service details to be injected via ASP.NET 5's configuration and dependency injection mechanisms.
