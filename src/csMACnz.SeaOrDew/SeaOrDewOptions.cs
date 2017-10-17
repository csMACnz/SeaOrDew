using System.Collections.Generic;
using System.Reflection;

namespace csMACnz.SeaOrDew
{
    public class SeaOrDewOptions
    {
        private List<Assembly> _commandAssemblies = new List<Assembly>();
        private List<Assembly> _queryAssemblies = new List<Assembly>();

        public IEnumerable<Assembly> CommandHandlerAssemblies { get { return _commandAssemblies; } }
        public IEnumerable<Assembly> QueryHandlerAssemblies { get { return _queryAssemblies; } }

        public void AutoLoadQueryHandlersFromAssembly(Assembly assembly)
        {
            _queryAssemblies.Add(assembly);
        }

        public void AutoLoadCommandHandlersFromAssembly(Assembly assembly)
        {
            _commandAssemblies.Add(assembly);
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
}