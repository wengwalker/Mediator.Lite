using Mediator.Lite.Interfaces;

namespace Mediator.Lite.Tests.UnitTests;

#region SpecificException

public class SpecificExceptionRequest : IRequest
{
    public string Message { get; set; } = string.Empty;
}

public class SpecificExceptionHandler : IRequestHandler<SpecificExceptionRequest>
{
    public Task Handle(SpecificExceptionRequest request, CancellationToken cancellationToken)
    {
        throw new ArgumentException("Specific test exception");
    }
}

#endregion SpecificException

#region LongRunning

public class LongRunningRequest : IRequest
{
    public int DelayMs { get; set; }
}

public class LongRunningRequestHandler : IRequestHandler<LongRunningRequest>
{
    public async Task Handle(LongRunningRequest request, CancellationToken cancellationToken)
    {
        await Task.Delay(request.DelayMs, cancellationToken).ConfigureAwait(false);
    }
}

#endregion LongRunning

#region MultiInterface

public class MultiInterfaceRequest1 : IRequest
{
    public string Data1 { get; set; } = string.Empty;
}

public class MultiInterfaceRequest2 : IRequest
{
    public string Data2 { get; set; } = string.Empty;
}

public class MultiInterfaceHandler : IRequestHandler<MultiInterfaceRequest1>, IRequestHandler<MultiInterfaceRequest2>
{
    public Task Handle(MultiInterfaceRequest1 request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task Handle(MultiInterfaceRequest2 request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

#endregion MultiInterface

#region Test

public class TestRequest : IRequest
{
    public string Name { get; set; } = string.Empty;
    public int Value { get; set; }
}

public class TestRequestHandler : IRequestHandler<TestRequest>
{
    public Task Handle(TestRequest request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

#endregion Test

#region TestWithResponse

public class TestRequestWithResponse : IRequest<TestResponse>
{
    public string Input { get; set; } = string.Empty;
    public int Number { get; set; }
}

public class TestResponse
{
    public string Result { get; set; } = string.Empty;
    public bool Success { get; set; }
    public int ProcessedValue { get; set; }
}

public class TestRequestWithResponseHandler : IRequestHandler<TestRequestWithResponse, TestResponse>
{
    public Task<TestResponse> Handle(TestRequestWithResponse request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new TestResponse
        {
            Result = $"Processed: {request.Input}",
            Success = true,
            ProcessedValue = request.Number * 2
        });
    }
}

#endregion TestWithResponse
