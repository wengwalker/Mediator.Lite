using AutoFixture.Xunit2;
using Mediator.Lite.Interfaces;
using Mediator.Lite.Tests.Attributes;
using Moq;

namespace Mediator.Lite.Tests.UnitTests;

public class MediatorTests
{
    [Theory, AutoMoqData]
    public async Task Send_WithValidRequest_ShouldCallHandler(
        TestRequest request,
        [Frozen] Mock<IServiceProvider> serviceProviderMock,
        [Frozen] Mock<IRequestHandler<TestRequest>> requestHandlerMock,
        Mediator mediator)
    {
        // Arrange
        serviceProviderMock
            .Setup(sp => sp.GetService(typeof(IRequestHandler<TestRequest>)))
            .Returns(requestHandlerMock.Object);

        requestHandlerMock
            .Setup(h => h.Handle(request, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await mediator.Send(request, It.IsAny<CancellationToken>());

        // Assert
        requestHandlerMock
            .Verify(h => h.Handle(request, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory, AutoMoqData]
    public async Task Send_WithNullRequest_ShouldThrowArgumentNullException(
        Mediator mediator)
    {
        // Act
        var act = () => mediator.Send(null!, It.IsAny<CancellationToken>());

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(act);
    }

    [Theory, AutoMoqData]
    public async Task Send_WithNoHandlerFound_ShouldThrowInvalidOperationException(
        TestRequest request,
        [Frozen] Mock<IServiceProvider> serviceProviderMock,
        Mediator mediator)
    {
        // Arrange
        serviceProviderMock
            .Setup(sp => sp.GetService(typeof(IRequestHandler<TestRequest>)))
            .Returns(null);

        // Act
        var act = () => mediator.Send(request, It.IsAny<CancellationToken>());

        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(act);
    }

    [Theory, AutoMoqData]
    public async Task Send_WithGenericRequest_ShouldCallHandlerAndReturnResponse(
        TestResponse response,
        TestRequestWithResponse request,
        [Frozen] Mock<IServiceProvider> serviceProviderMock,
        [Frozen] Mock<IRequestHandler<TestRequestWithResponse, TestResponse>> requestHandlerMock,
        Mediator mediator)
    {
        // Arrange
        serviceProviderMock
            .Setup(sp => sp.GetService(typeof(IRequestHandler<TestRequestWithResponse, TestResponse>)))
            .Returns(requestHandlerMock.Object);

        requestHandlerMock
            .Setup(h => h.Handle(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await mediator.Send(request, It.IsAny<CancellationToken>());

        // Assert
        Assert.Equal(response, result);

        requestHandlerMock
            .Verify(h => h.Handle(request, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory, AutoMoqData]
    public async Task Send_WithGenericRequestAndNullRequest_ShouldThrowArgumentNullException(
        Mediator mediator)
    {
        // Act
        var act = () => mediator.Send<TestResponse>(null!, It.IsAny<CancellationToken>());

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(act);
    }

    [Theory, AutoMoqData]
    public async Task Send_WithGenericRequestAndNoHandlerFound_ShouldThrowInvalidOperationException(
        TestRequestWithResponse request,
        [Frozen] Mock<IServiceProvider> serviceProviderMock,
        Mediator mediator)
    {
        // Arrange
        serviceProviderMock
            .Setup(sp => sp.GetService(typeof(IRequestHandler<TestRequestWithResponse, TestResponse>)))
            .Returns(null);

        // Act
        var act = () => mediator.Send(request, It.IsAny<CancellationToken>());

        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(act);
    }

    [Theory, AutoMoqData]
    public async Task Send_WithSameRequestType_ShouldUseCachedHandlerType(
        TestRequest request1,
        TestRequest request2,
        [Frozen] Mock<IServiceProvider> serviceProviderMock,
        [Frozen] Mock<IRequestHandler<TestRequest>> requestHandlerMock,
        Mediator mediator)
    {
        // Arrange
        serviceProviderMock
            .Setup(sp => sp.GetService(typeof(IRequestHandler<TestRequest>)))
            .Returns(requestHandlerMock.Object);

        requestHandlerMock
            .Setup(h => h.Handle(It.IsAny<TestRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await mediator.Send(request1, It.IsAny<CancellationToken>());
        await mediator.Send(request2, It.IsAny<CancellationToken>());

        // Assert
        serviceProviderMock
            .Verify(sp => sp.GetService(typeof(IRequestHandler<TestRequest>)), Times.Exactly(2));

        requestHandlerMock
            .Verify(h => h.Handle(It.IsAny<TestRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Theory, AutoMoqData]
    public async Task Send_WithHandlerThrowingException_ShouldPropagateException(
        TestRequest request,
        [Frozen] Mock<IServiceProvider> serviceProviderMock,
        [Frozen] Mock<IRequestHandler<TestRequest>> requestHandlerMock,
        Mediator mediator)
    {
        // Arrange
        serviceProviderMock
            .Setup(sp => sp.GetService(typeof(IRequestHandler<TestRequest>)))
            .Returns(requestHandlerMock.Object);

        requestHandlerMock
            .Setup(h => h.Handle(request, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException());

        // Act
        var act = () => mediator.Send(request, It.IsAny<CancellationToken>());

        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(act);
    }
}

