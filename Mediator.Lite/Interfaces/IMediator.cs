namespace Mediator.Lite.Interfaces;

public interface IMediator
{
    Task Send(IRequest request, CancellationToken cancellationToken);

    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken);
}