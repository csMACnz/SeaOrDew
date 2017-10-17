using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace csMACnz.SeaOrDew
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSeaOrDewHandlers(this IServiceCollection services, SeaOrDewOptions options = null)
        {
            services.AddSingleton<QueryHandler>();
            services.AddSingleton<CommandHandler>();
            if(options != null)
            {
                foreach(var assembly in options.CommandHandlerAssemblies)
                {
                    AddInstancesOfGenericTypeDefinition(services, assembly, typeof(ICommandHandler<,>), ServiceLifetime.Scoped);        
                }
                foreach(var assembly in options.QueryHandlerAssemblies)
                {
                    AddInstancesOfGenericTypeDefinition(services, assembly, typeof(ICommandHandler<,>), ServiceLifetime.Scoped);        
                }
            }
        }

        private static void AddInstancesOfGenericTypeDefinition(IServiceCollection services, Assembly assembly, Type genericTypeDefinition, ServiceLifetime lifetime)
        {
            var matches = 
                GetLoadableTypes(assembly)
                .Where(t => t.GetTypeInfo().ImplementedInterfaces.Any(i => InterfaceMatchesGenericTypeDefinition(i, genericTypeDefinition)))
                .Select(t =>
                new
                {
                    HandlerType = t,
                    Interface = t.GetTypeInfo().ImplementedInterfaces.FirstOrDefault(i => InterfaceMatchesGenericTypeDefinition(i, genericTypeDefinition))
                });

            foreach (var matcheResult in matches)
            {
                services.TryAdd(new ServiceDescriptor(matcheResult.Interface, matcheResult.HandlerType, lifetime));
            }
        }

        private static bool InterfaceMatchesGenericTypeDefinition(Type interfaceType, Type genericTypeDefinition)
        {
            return interfaceType.GetTypeInfo().IsGenericType && interfaceType.GetGenericTypeDefinition() == genericTypeDefinition;
        }

        private static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            try
            {
                return assembly.GetLoadableTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }
    }
}