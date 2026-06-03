using Mediator.Lite.Extensions;
using Mediator.Lite.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Mediator.Lite.Tests.UnitTests;

public class MediatorServicesExtensionsTests
{
    [Fact]
    public void AddMediator_WithValidAssembly_ShouldRegisterMediator()
    {
        // Arrange
        var services = new ServiceCollection();
        var assembly = typeof(TestRequestHandler).Assembly;

        // Act
        var result = services.AddMediator(assembly);

        // Assert
        Assert.Same(services, result);

        var serviceProvider = services.BuildServiceProvider();
        var mediator = serviceProvider.GetService<IMediator>();
        Assert.NotNull(mediator);
        Assert.IsType<Mediator>(mediator);
    }

    [Fact]
    public void AddMediator_WithValidAssembly_ShouldRegisterHandlers()
    {
        // Arrange
        var services = new ServiceCollection();
        var assembly = typeof(TestRequestHandler).Assembly;

        // Act
        services.AddMediator(assembly);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var handler = serviceProvider.GetService<IRequestHandler<TestRequest>>();

        Assert.NotNull(handler);
        Assert.IsType<TestRequestHandler>(handler);
    }

    [Fact]
    public void AddMediator_WithAssemblyContainingNoHandlers_ShouldOnlyRegisterMediator()
    {
        // Arrange
        var services = new ServiceCollection();
        var assembly = typeof(string).Assembly;

        // Act
        services.AddMediator(assembly);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var mediator = serviceProvider.GetService<IMediator>();
        Assert.NotNull(mediator);
    }

    [Fact]
    public void AddMediator_WithMultipleCalls_ShouldNotDuplicateRegistrations()
    {
        // Arrange
        var services = new ServiceCollection();
        var assembly = typeof(TestRequestHandler).Assembly;

        // Act
        services.AddMediator(assembly);
        services.AddMediator(assembly);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var mediator = serviceProvider.GetService<IMediator>();
        Assert.NotNull(mediator);

        var handler = serviceProvider.GetService<IRequestHandler<TestRequest>>();
        Assert.NotNull(handler);
    }

    [Fact]
    public void AddMediator_WithNullServices_ShouldThrowArgumentNullException()
    {
        // Arrange
        IServiceCollection? services = null;
        var assembly = typeof(TestRequestHandler).Assembly;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => services!.AddMediator(assembly));
    }

    [Fact]
    public void AddMediator_WithNullAssembly_ShouldThrowNullReferenceException()
    {
        // Arrange
        var services = new ServiceCollection();
        Assembly? assembly = null;

        // Act
        var act = () => services.AddMediator(assembly!);

        // Assert
        Assert.Throws<ArgumentNullException>(act);
    }

    [Fact]
    public void AddMediator_WithHandlerImplementingMultipleInterfaces_ShouldRegisterAllInterfaces()
    {
        // Arrange
        var services = new ServiceCollection();

        var assembly = typeof(MultiInterfaceHandler).Assembly;

        // Act
        services.AddMediator(assembly);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var handler1 = serviceProvider.GetService<IRequestHandler<MultiInterfaceRequest1>>();
        var handler2 = serviceProvider.GetService<IRequestHandler<MultiInterfaceRequest2>>();

        Assert.NotNull(handler1);
        Assert.NotNull(handler2);
        Assert.IsType<MultiInterfaceHandler>(handler1);
        Assert.IsType<MultiInterfaceHandler>(handler2);
    }
}
