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

        public void AutoLoadQueryHandlersFromAssembly(Assembly assembly)
        {
            _queryAssemblies.Add(new HandlerSource(assembly));
        }

        public void AutoLoadCommandHandlersFromAssembly(Assembly assembly)
        {
            _commandAssemblies.Add(new HandlerSource(assembly));
        }

        public void UseLifetimeScope(ServiceLifetime lifetime)
        {
            ServiceLifetime = lifetime;
        }
    }

    public static class SeaOrDewOptionsExtensions
    {
        public static void LoadAllHandlersFromAssembly(this SeaOrDewOptions options, Assembly assembly)
        {
            options.AutoLoadCommandHandlersFromAssembly(assembly);
            options.AutoLoadQueryHandlersFromAssembly(assembly);
        }
    }

    public class HandlerSource
    {
        public HandlerSource(Assembly assembly)
        {
            //TODO: add Namespace filter options, class name prefix, suffix filters?
            Assembly = assembly;
        }

        public Assembly Assembly { get; }
    }
}