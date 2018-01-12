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
            var descriptors = new[] {
                new ServiceDescriptor(typeof(QueryHandler), typeof(QueryHandler), options?.ServiceLifetime??ServiceLifetime.Scoped),
                new ServiceDescriptor(typeof(CommandHandler), typeof(CommandHandler), options?.ServiceLifetime??ServiceLifetime.Scoped)
            };

            if (options?.TryAdd == true)
            {
                services.TryAdd(descriptors);
            }
            else
            {
                services.Add(descriptors);
            }
            if (options != null)
            {
                foreach (var handlerSource in options.CommandHandlerAssemblies)
                {
                    AddInstancesOfGenericTypeDefinition(services, handlerSource.Assembly, handlerSource.NamespacePrefix, typeof(ICustomCommandHandler<,>), options.ServiceLifetime, options.TryAdd);
                }
                foreach (var handlerSource in options.QueryHandlerAssemblies)
                {
                    AddInstancesOfGenericTypeDefinition(services, handlerSource.Assembly, handlerSource.NamespacePrefix, typeof(IQueryHandler<,>), options.ServiceLifetime, options.TryAdd);
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
            var interfaceType = targetType.GetTypeInfo().GetConcreteInterfaceImplementationForGenericInterface(genericTypeDefinition);
            if (interfaceType is null)
            {
                throw new NotSupportedException($"The type {targetType} doesn't implement {genericTypeDefinition}.");
            }

            services.Add(new ServiceDescriptor(interfaceType, targetType, lifetime));
            return services;
        }

        private static void AddInstancesOfGenericTypeDefinition(IServiceCollection services, Assembly assembly, string namespacePrefix, Type genericTypeDefinition, ServiceLifetime lifetime, bool tryAdd)
        {
            var matches =
                GetAccessibleTypes(assembly)
                .Select(t =>
                new
                {
                    HandlerType = t,
                    Interface = t.GetConcreteInterfaceImplementationForGenericInterface(genericTypeDefinition)
                })
                .Where(t =>
                    t.Interface != null &&
                    (string.IsNullOrEmpty(namespacePrefix) || t.HandlerType.Namespace.StartsWith(namespacePrefix)));

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