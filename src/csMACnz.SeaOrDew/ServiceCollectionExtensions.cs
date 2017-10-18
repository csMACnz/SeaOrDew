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
                    AddInstancesOfGenericTypeDefinition(services, handlerSource.Assembly, typeof(ICustomCommandHandler<,>), options.ServiceLifetime, options.TryAdd);
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
            var interfaceType = GetConcreteInterfaceImplementationType(targetType, genericTypeDefinition);
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
                GetLoadableTypes(assembly)
                .Where(t => TypeImplementsGenericInterface(t, genericTypeDefinition))
                .Select(t =>
                new
                {
                    HandlerType = t,
                    Interface = GetConcreteInterfaceImplementationType(t, genericTypeDefinition)
                });

            foreach (var matcheResult in matches)
            {
                var descriptor = new ServiceDescriptor(matcheResult.Interface, matcheResult.HandlerType, lifetime);
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

        private static bool TypeImplementsGenericInterface(Type type, Type genericTypeDefinition)
        {
            return null != GetConcreteInterfaceImplementationType(type, genericTypeDefinition);
        }

        private static Type GetConcreteInterfaceImplementationType(Type type, Type genericTypeDefinition)
        {
            return type.GetTypeInfo().ImplementedInterfaces.FirstOrDefault(i => InterfaceMatchesGenericTypeDefinition(i, genericTypeDefinition));
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