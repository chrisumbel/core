# SteelToe OSS Core
SteelToe is a collection of libraries designed to facilitate the consumption of several OSS packages such as **Spring Cloud Configuration Server**, **Eureka** (service discovery and registration), as well as provide .NET implementations for some Netflix OSS components like **Circuit Breakers**.

In this repository, you'll find 3 projects:

## SteelToe.Core
This is the main library. Reference this package to gain access to the building blocks for SteelToe, including code that will seamlessly inject **Cloud Foundry** bound service details into your application.

To build, go to the src/SteelToe.Core directory:
`dnu restore`
`dnu build`
`dnu install`

## SteelToe.Core.Test
Unit and integration tests for the **SteelToe.Core** library.

To run the tests, make sure you've already built and installed core, and go to the src/SteelToe.Core.Test directory:

`dnx test`

## SteelToe.Core.Sample
A sample service written in ASP.NET 5 to illustrate the recommended best practice for enabling cloud foundry bound service details to be injected via ASP.NET 5's configuration and dependency injection mechanisms.

Using the .NET command line (**dnx** and **dnu** at the moment, **dotnet** in the near future), build and install the **SteelToe.Core** library and then you can run the sample:

`dnx web`

Using whatever tool you like, issue a `GET http://localhost:5000/services` and this will display a list of services detected by parsing the **VCAP_SERVICES** environment variable (or manually injected configuration property).
