using System;
using Xunit;

namespace csMACnz.SeaOrDew.Tests
{
    public class ServiceCollectionExtensionsTests
    {
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
    }
}