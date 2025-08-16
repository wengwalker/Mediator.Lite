using Mediator.Lite.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Mediator.Lite.Extensions;

public static class MediatorServicesExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services, Assembly assembly)
    {
        services.TryAddTransient<IMediator, Mediator>();

        return AddMediatorHandlers(services, assembly);
    }

    private static IServiceCollection AddMediatorHandlers(IServiceCollection services, Assembly assembly)
    {
        var handlerTypes = assembly.GetTypes()
            .Where(t =>
                t.GetInterfaces().Any(i =>
                  i.IsGenericType &&
                  (i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>) ||
                  i.GetGenericTypeDefinition() == typeof(IRequestHandler<>))));

        RegisterHandlers(services, typeof(IRequestHandler<,>), handlerTypes);
        RegisterHandlers(services, typeof(IRequestHandler<>), handlerTypes);

        return services;
    }

    private static void RegisterHandlers(IServiceCollection services, Type type, IEnumerable<Type> handlerTypes)
    {
        foreach (var handlerType in handlerTypes)
        {
            var handlerInterfaces = handlerType
                .GetInterfaces()
                .Where(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == type);

            foreach (var handlerInterface in handlerInterfaces)
            {
                services.TryAddTransient(handlerInterface, handlerType);
            }
        }
    }
}