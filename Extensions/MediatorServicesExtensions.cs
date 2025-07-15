using Mediator.Lite.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Mediator.Lite.Extensions;

public static class MediatorServicesExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services.AddTransient<IMediator, Mediator>();

        AddMediatorHandlers(services, Assembly.GetExecutingAssembly());

        return services;
    }

    private static void AddMediatorHandlers(IServiceCollection services, Assembly assembly)
    {
        var handlerTypes = assembly.GetTypes()
            .Where(t =>
                t.GetInterfaces().Any(i =>
                  i.IsGenericType &&
                  i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)));

        foreach (var handlerType in handlerTypes)
        {
            var handlerInterfaces = handlerType
                .GetInterfaces()
                .Where(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));

            foreach (var handlerInterface in handlerInterfaces)
            {
                services.AddTransient(handlerInterface, handlerType);
            }
        }
    }
}