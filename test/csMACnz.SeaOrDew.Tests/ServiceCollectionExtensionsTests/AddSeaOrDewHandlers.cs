using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using Xunit;

namespace csMACnz.SeaOrDew.Tests.ServiceCollectionExtensionsTests
{
    public class AddSeaOrDewHandlers
    {
        [Fact]
        public void NoOptions_CorrectlyAddsMainServices()
        {
            var fakeCollection = new TestServiceCollection();
            fakeCollection.AddSeaOrDewHandlers();

            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(CommandHandler));
            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(QueryHandler));
        }

        [Fact]
        public void NullOptions_CorrectlyAddsMainServices()
        {
            var fakeCollection = new TestServiceCollection();
            fakeCollection.AddSeaOrDewHandlers((SeaOrDewOptions)null);

            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(CommandHandler));
            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(QueryHandler));
        }

        [Fact]
        public void PassOptions_CorrectlyAddsMainServices()
        {
            var fakeCollection = new TestServiceCollection();
            fakeCollection.AddSeaOrDewHandlers(new SeaOrDewOptions());

            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(CommandHandler));
            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(QueryHandler));
        }

        [Fact]
        public void NullLambda_CorrectlyAddsMainServices()
        {
            var fakeCollection = new TestServiceCollection();
            fakeCollection.AddSeaOrDewHandlers((Action<SeaOrDewOptions>)null);

            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(CommandHandler));
            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(QueryHandler));
        }

        [Fact]
        public void OptionsLambda_CorrectlyAddsMainServices()
        {
            var fakeCollection = new TestServiceCollection();
            fakeCollection.AddSeaOrDewHandlers(options =>
            {

            });

            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(CommandHandler));
            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(QueryHandler));
        }

        [Fact]
        public void NoOptions_DefaultLifetimeIsScoped()
        {
            var fakeCollection = new TestServiceCollection();
            fakeCollection.AddSeaOrDewHandlers();

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
        public void LifetimeOption_CorrectlyAddsMainServices(ServiceLifetime lifetime)
        {
            var fakeCollection = new TestServiceCollection();
            fakeCollection.AddSeaOrDewHandlers(options =>
            {
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