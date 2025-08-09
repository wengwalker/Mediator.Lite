Mediator.Lite
=============

![License](https://img.shields.io/github/license/wengwalker/Mediator.Lite)
[![Version](https://img.shields.io/nuget/v/Mediator.Lite)](https://www.nuget.org/packages/Mediator.Lite)
[![Workflow Status](https://img.shields.io/github/actions/workflow/status/wengwalker/Mediator.Lite/publish-nuget.yml)](https://github.com/wengwalker/Mediator.Lite/actions)

Mediator.Lite - Lightweight implementation of mediator for .NET

### How to start

You need to install Nuget package: [link](https://www.nuget.org/packages/Mediator.Lite)

You can do this with the following commands:
```csharp
    Install-Package Mediator.Lite
```
Or with .NET CLI:
```csharp
    dotnet add package Mediator.Lite
```

### Using with Dependency Injection

You need to register handlers through this extension method for `IServiceCollection`:
```csharp
    services.AddMediator();
```
When the `AddMediator()` method is called, it searches for classes that implement the `IRequestHandler` interface in the current assembly.

### A few words

This library does not pretend to compete with the popular MediatR library, and does not try to borrow its code in any way. This library was made only for the purpose of using it in its own developments by its author (that is, me)