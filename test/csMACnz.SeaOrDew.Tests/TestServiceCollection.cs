using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace csMACnz.SeaOrDew.Tests
{
    internal class TestServiceCollection: List<ServiceDescriptor>, IServiceCollection
    {
        public TestServiceCollection()
        {
        }
    }
}