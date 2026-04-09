using Mediator.Lite.Extensions;
using Mediator.Lite.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Reflection;

namespace Mediator.Lite.Tests.UnitTests;

public class EdgeCaseTests
{
    [Fact]
    public void Mediator_WithEmptyAssembly_ShouldNotThrow()
    {
        // Arrange
        var services = new ServiceCollection();

        services.AddMediator(typeof(string).Assembly);

        var serviceProvider = services.BuildServiceProvider();

        // Act
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        // Assert
        Assert.NotNull(mediator);
    }

    [Fact]
    public async Task Mediator_WithHandlerThrowingSpecificException_ShouldPropagateCorrectException()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMediator(typeof(EdgeCaseTests).Assembly);

        var serviceProvider = services.BuildServiceProvider();
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        // Act
        var act = () => mediator.Send(new SpecificExceptionRequest(), It.IsAny<CancellationToken>());

        // Assert
        await Assert.ThrowsAsync<TargetInvocationException>(act);
    }

    [Fact]
    public async Task Mediator_WithCancelledToken_ShouldThrowOperationCanceledException()
    {
        // Arrange
        var services = new ServiceCollection();

        services.AddMediator(typeof(EdgeCaseTests).Assembly);

        var serviceProvider = services.BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var cts = new CancellationTokenSource();

        // Act
        cts.Cancel();
        var act = () => mediator.Send(new LongRunningRequest(), cts.Token);

        // Act & Assert
        await Assert.ThrowsAsync<TaskCanceledException>(act);
    }

    [Fact]
    public async Task Mediator_WithHandlerImplementingMultipleGenericInterfaces_ShouldWorkCorrectly()
    {
        // Arrange
        var services = new ServiceCollection();

        services.AddMediator(typeof(EdgeCaseTests).Assembly);

        var serviceProvider = services.BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();

        // Act
        await mediator.Send(new MultiInterfaceRequest1(), It.IsAny<CancellationToken>());
        await mediator.Send(new MultiInterfaceRequest2(), It.IsAny<CancellationToken>());

        // Assert
    }
}
