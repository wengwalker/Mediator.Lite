using Mediator.Lite.Interfaces;
using System.Collections.Concurrent;

namespace Mediator.Lite;

public class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    private static readonly ConcurrentDictionary<Type, Type> RequestHandlerTypes = new();

    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var requestType = request.GetType();

        var handlerType = RequestHandlerTypes.GetOrAdd(requestType,
            t => typeof(IRequestHandler<,>).MakeGenericType(t, typeof(TResponse)));

        var handler = _serviceProvider.GetService(handlerType)
            ?? throw new InvalidOperationException($"Handler not found for {requestType.Name}");

        return (Task<TResponse>)handlerType.GetMethod("Handle")!
            .Invoke(handler, [ request, cancellationToken ])!;
    }
}