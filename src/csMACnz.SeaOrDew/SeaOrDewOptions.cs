using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace csMACnz.SeaOrDew
{
    public class SeaOrDewOptions
    {
        private List<HandlerSource> _commandAssemblies = new List<HandlerSource>();
        private List<HandlerSource> _queryAssemblies = new List<HandlerSource>();

        public IEnumerable<HandlerSource> CommandHandlerAssemblies { get { return _commandAssemblies; } }
        public IEnumerable<HandlerSource> QueryHandlerAssemblies { get { return _queryAssemblies; } }

        public ServiceLifetime ServiceLifetime { get; private set; }

        public bool TryAdd { get; set; }

        public void LoadQueryHandlers(HandlerSource source)
        {
            _queryAssemblies.Add(source);
        }

        public void LoadCommandHandlers(HandlerSource source)
        {
            _commandAssemblies.Add(source);
        }

        public void UseLifetimeScope(ServiceLifetime lifetime)
        {
            ServiceLifetime = lifetime;
        }
    }

    public static class SeaOrDewOptionsExtensions
    {
        public static void LoadCommandHandlersFromAssembly(
            this SeaOrDewOptions options,
            Assembly assembly)
        {
            options.LoadCommandHandlers(new HandlerSource(assembly));
        }

        public static void LoadQueryHandlersFromAssembly(
            this SeaOrDewOptions options,
            Assembly assembly)
        {
            options.LoadQueryHandlers(new HandlerSource(assembly));
        }

        public static void LoadAllHandlersFromAssembly(
            this SeaOrDewOptions options,
            Assembly assembly)
        {
            options.LoadCommandHandlersFromAssembly(assembly);
            options.LoadQueryHandlersFromAssembly(assembly);
        }

        public static void LoadCommandHandlersFromAssemblyUnderNamespace(
            this SeaOrDewOptions options,
            Assembly assembly,
            string namespacePrefix)
        {
            options.LoadCommandHandlers(new HandlerSource(assembly, namespacePrefix));
        }

        public static void LoadQueryHandlersFromAssemblyUnderNamespace(
            this SeaOrDewOptions options,
            Assembly assembly,
            string namespacePrefix)
        {
            options.LoadQueryHandlers(new HandlerSource(assembly, namespacePrefix));
        }

        public static void LoadAssemblyHandlersUnderNamespace(
            this SeaOrDewOptions options,
            Assembly assembly,
            string namespacePrefix)
        {
            options.LoadCommandHandlersFromAssemblyUnderNamespace(assembly, namespacePrefix);
            options.LoadQueryHandlersFromAssemblyUnderNamespace(assembly, namespacePrefix);
        }
    }

    public class HandlerSource
    {
        public HandlerSource(Assembly assembly)
            :this(assembly, null)
        {
        }

        public HandlerSource(Assembly assembly, string namespacePrefix)
        {
            //TODO: add Namespace filter options, class name prefix, suffix filters?
            Assembly = assembly;
            NamespacePrefix = namespacePrefix;
        }

        public Assembly Assembly { get; }
        public string NamespacePrefix { get; }
    }
}