using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using Xunit;

namespace csMACnz.SeaOrDew.Tests
{
    public class ServiceCollectionExtensionsTests
    {
        public const int OtherTestsHandlersCount = 5;
        public const int SetACommandCount = 3;
        public const int SetAQueryCount = 0;
        public const int SetBCommandCount = 2;
        public const int SetBQueryCount = 0;
        public const int SetACount = SetACommandCount + SetAQueryCount;
        public const int SetBCount = SetBCommandCount +SetBQueryCount;
        public const int AssemblyCommandHandlerCount = SetACommandCount + SetBCommandCount;
        public const int AssemblyQueryHandlerCount = SetAQueryCount + SetBQueryCount;
        public const int AssemblyHandlerCount = SetACount + SetBCount;
        public static readonly Assembly TestHandlersAssembly = typeof(TestHandlers.SetA.CustomTestCommand).GetTypeInfo().Assembly;

        [Fact]
        public void AddHandlerExtension_NoOptions_CorrectlyAddsMainServices()
        {
            var fakeCollection = new TestServiceCollection();
            fakeCollection.AddSeaOrDewHandlers();

            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(CommandHandler));
            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(QueryHandler));
        }

        [Fact]
        public void AddHandlerExtension_NullOptions_CorrectlyAddsMainServices()
        {
            var fakeCollection = new TestServiceCollection();
            fakeCollection.AddSeaOrDewHandlers((SeaOrDewOptions)null);

            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(CommandHandler));
            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(QueryHandler));
        }

        [Fact]
        public void AddHandlerExtension_PassOptions_CorrectlyAddsMainServices()
        {
            var fakeCollection = new TestServiceCollection();
            fakeCollection.AddSeaOrDewHandlers(new SeaOrDewOptions());

            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(CommandHandler));
            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(QueryHandler));
        }
        
        [Fact]
        public void AddHandlerExtension_NullLambda_CorrectlyAddsMainServices()
        {
            var fakeCollection = new TestServiceCollection();
            fakeCollection.AddSeaOrDewHandlers((Action<SeaOrDewOptions>)null);

            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(CommandHandler));
            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(QueryHandler));
        }

        [Fact]
        public void AddHandlerExtension_OptionsLambda_CorrectlyAddsMainServices()
        {
            var fakeCollection = new TestServiceCollection();
            fakeCollection.AddSeaOrDewHandlers(options=>
            {
                
            });

            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(CommandHandler));
            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(QueryHandler));
        }

        [Fact]
        public void AddHandlerExtension_NoOptions_DefaultLifetimeIsScoped()
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
        public void AddHandlerExtension_LifetimeOption_CorrectlyAddsMainServices(ServiceLifetime lifetime)
        {
            var fakeCollection = new TestServiceCollection();
            fakeCollection.AddSeaOrDewHandlers(options =>
            {
                options.UseLifetimeScope(lifetime);
            });

            Assert.All(fakeCollection, s => {
                if (s.ServiceType == typeof(CommandHandler)) return;
                if (s.ServiceType == typeof(QueryHandler)) return;
                Assert.Equal(lifetime, s.Lifetime);
            });
        }

        [Fact]
        public void AddHandlerExtension_LoadAllFromAssembly_CorrectlyAddsHandlers()
        {
            var fakeCollection = new TestServiceCollection();
            fakeCollection.AddSeaOrDewHandlers(options =>
            {
                options.LoadAllHandlersFromAssembly(TestHandlersAssembly);
            });

            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(CommandHandler));
            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(QueryHandler));
            Assert.Equal(OtherTestsHandlersCount + AssemblyHandlerCount + 2, fakeCollection.Count);
        }
    }
}