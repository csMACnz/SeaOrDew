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
        public static IServiceCollection AddSeaOrDewHandlers(this IServiceCollection services, Action<SeaOrDewOptions> optionsSetup)
        {
            var options = new SeaOrDewOptions();
            optionsSetup?.Invoke(options);
            return services.AddSeaOrDewHandlers(options);
        }

        public static IServiceCollection AddSeaOrDewHandlers(this IServiceCollection services, SeaOrDewOptions options = null)
        {
            services.AddSingleton<QueryHandler>();
            services.AddSingleton<CommandHandler>();
            if (options != null)
            {
                foreach (var handlerSource in options.CommandHandlerAssemblies)
                {
                    AddInstancesOfGenericTypeDefinition(services, handlerSource.Assembly, typeof(ICustomCommandHandler<,>), options.ServiceLifetime, options.TryAdd);
                }
                foreach (var handlerSource in options.QueryHandlerAssemblies)
                {
                    AddInstancesOfGenericTypeDefinition(services, handlerSource.Assembly, typeof(IQueryHandler<,>), options.ServiceLifetime, options.TryAdd);
                }
            }
            return services;
        }

        public static IServiceCollection RegisterQueryHandler<TImpl>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            var targetType = typeof(TImpl);
            var genericTypeDefinition = typeof(IQueryHandler<,>);
            return RegisterHandler(services, targetType, genericTypeDefinition, lifetime);
        }

        public static IServiceCollection RegisterCommandHandler<TImpl>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            var targetType = typeof(TImpl);
            var genericTypeDefinition = typeof(ICustomCommandHandler<,>);
            return RegisterHandler(services, targetType, genericTypeDefinition, lifetime);
        }

        private static IServiceCollection RegisterHandler(IServiceCollection services, Type targetType, Type genericTypeDefinition, ServiceLifetime lifetime)
        {
            var interfaceType = GetConcreteInterfaceImplementationType(targetType.GetTypeInfo(), genericTypeDefinition);
            if (interfaceType is null)
            {
                throw new NotSupportedException($"The type {targetType} doesn't implement {genericTypeDefinition}.");
            }

            services.Add(new ServiceDescriptor(interfaceType, targetType, lifetime));
            return services;
        }

        private static void AddInstancesOfGenericTypeDefinition(IServiceCollection services, Assembly assembly, Type genericTypeDefinition, ServiceLifetime lifetime, bool tryAdd)
        {
            var matches = 
                GetAccessibleTypes(assembly)
                .Select(t =>
                new
                {
                    HandlerType = t,
                    Interface = GetConcreteInterfaceImplementationType(t, genericTypeDefinition)
                })
                .Where(t => t.Interface != null);

            foreach (var matcheResult in matches)
            {
                var descriptor = new ServiceDescriptor(matcheResult.Interface, matcheResult.HandlerType.AsType(), lifetime);
                if (tryAdd)
                {
                    services.TryAdd(descriptor);
                }
                else
                {
                    services.Add(descriptor);
                }
            }
        }
        
        private static Type GetConcreteInterfaceImplementationType(TypeInfo type, Type genericTypeDefinition)
        {
            return type.ImplementedInterfaces.FirstOrDefault(i => InterfaceMatchesGenericTypeDefinition(i, genericTypeDefinition));
        }

        private static bool InterfaceMatchesGenericTypeDefinition(Type interfaceType, Type genericTypeDefinition)
        {
            return interfaceType.GetTypeInfo().IsGenericType && interfaceType.GetGenericTypeDefinition() == genericTypeDefinition;
        }

        private static IEnumerable<TypeInfo> GetAccessibleTypes(this Assembly assembly)
        {
            try
            {
                return assembly.DefinedTypes;
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types.Where(t => t != null).Select(t => t.GetTypeInfo());
            }
        }

    }
}