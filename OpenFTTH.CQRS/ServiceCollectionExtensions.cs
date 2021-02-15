using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OpenFTTH.CQRS
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCQRS(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
            services.AddSingleton<IQueryDispatcher, QueryDispatcher>();

            foreach (var assembly in assemblies)
            {
                RegisterHandlersOfType(services, assembly, typeof(ICommandHandler<,>));
                RegisterHandlersOfType(services, assembly, typeof(IQueryHandler<,>));
            }
        }

        private static void RegisterHandlersOfType(this IServiceCollection services, Assembly assembly, Type handlerInterface)
        {
            var handlers = assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface)
            );

            foreach (var handler in handlers)
            {
                services.AddScoped(handler.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface), handler);
            }
        }
    }
}
        
