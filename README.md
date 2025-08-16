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

### How to use

To use this library you need to create an handlers by realizing `IRequestHandler` and register them in DI-container.

To create a handler for the specified command, you need to create the following classes:
```csharp
public record YourCommand() : IRequest;

public class YourCommandHandler : IRequestHandler<YourCommand>
{
    public Task Handle(YourCommand request, CancellationToken cancellationToken)
    {
        // Implementation...
    }
}
```
Or if you need to return any response from handlers, you need to specify the response like this:
```csharp
public record YourCommandResponse();

public record YourCommand() : IRequest<YourCommandResponse>;

public class YourCommandHandler : IRequestHandler<YourCommand, YourCommandResponse>
{
    public Task<YourCommandResponse> Handle(YourCommand request, CancellationToken cancellationToken)
    {
        // Implementation...
    }
}
```
Then you need to register handlers through this extension method for `IServiceCollection` and specifying assembly to search:
```csharp
    services.AddMediator(typeof(YourCommandHandler).Assembly);
```
When the `AddMediator()` method is called, it registers the `Mediator` class for dependency resolving and looks for handler classes implementing the `IRequestHandler` interface (both `IRequestHandler<>` that does not return a response and `IRequestHandler<,>` that does return a response) in the specified assembly and registers them as dependencies.

### A few words

This library does not pretend to compete with the popular MediatR library, and does not try to borrow its code in any way. This library was made only for the purpose of using it in its own developments by its author (that is, me)