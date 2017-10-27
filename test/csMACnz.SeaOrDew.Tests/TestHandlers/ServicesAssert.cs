using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace csMACnz.SeaOrDew.Tests
{
    public static class ServicesAssert
    {
        public static void OnlyHandlerResolversRegistered(IEnumerable<ServiceDescriptor> services)
        {
            Assert.Collection(
                services,
            s =>
            {
                Assert.Equal(typeof(QueryHandler), s.ServiceType);
            },
            s =>
            {
                Assert.Equal(typeof(CommandHandler), s.ServiceType);
            });
        }
    }
}