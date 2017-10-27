using csMACnz.SeaOrDew.Tests;
using csMACnz.SeaOrDew.Tests.TestHandlers.SetA;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static csMACnz.SeaOrDew.Tests.TestHandlers.Stats;
using System.Linq;

namespace csMACnz.SeaOrDew.Tests
{
    public class UseLifetimeScope
    {
        [Fact]
        public void NoOptions_WithoutHandlers_OnlyDefaultsRegistered()
        {
            var fakeCollection = new TestServiceCollection();
            fakeCollection.AddSeaOrDewHandlers();

            ServicesAssert.OnlyHandlerResolversRegistered(fakeCollection);
        }

        [Theory]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        [InlineData(ServiceLifetime.Transient)]
        public void LifetimeOption_WithoutHandlers_OnlyDefaultsRegistered(ServiceLifetime lifetime)
        {
            var fakeCollection = new TestServiceCollection();
            fakeCollection.AddSeaOrDewHandlers(options =>
            {
                options.UseLifetimeScope(lifetime);
            });

            ServicesAssert.OnlyHandlerResolversRegistered(fakeCollection);
        }

        [Fact]
        public void NoLifeTimeOption_WithHandlers_DefaultLifetimeIsScoped()
        {
            var fakeCollection = new TestServiceCollection();
            fakeCollection.AddSeaOrDewHandlers(options =>
            {
                options.LoadCommandHandlersFromAssemblyUnderNamespace(TestHandlersAssembly, "csMACnz.SeaOrDew.Tests.TestHandlers.SetA");
            });

            Assert.All(fakeCollection, s =>
            {
                if (s.ServiceType == typeof(CommandHandler)) return;
                if (s.ServiceType == typeof(QueryHandler)) return;
                Assert.Equal(ServiceLifetime.Scoped, s.Lifetime);
            });
        }

        [Theory]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        [InlineData(ServiceLifetime.Transient)]
        public void LifetimeOption_WithHandlers_CorrectlyRegistersScope(ServiceLifetime lifetime)
        {
            var fakeCollection = new TestServiceCollection();
            fakeCollection.AddSeaOrDewHandlers(options =>
            {
                options.LoadCommandHandlersFromAssemblyUnderNamespace(TestHandlersAssembly, "csMACnz.SeaOrDew.Tests.TestHandlers.SetA");
                options.UseLifetimeScope(lifetime);
            });

            Assert.All(fakeCollection, s =>
            {
                if (s.ServiceType == typeof(CommandHandler)) return;
                if (s.ServiceType == typeof(QueryHandler)) return;
                Assert.Equal(lifetime, s.Lifetime);
            });
        }
    }
}