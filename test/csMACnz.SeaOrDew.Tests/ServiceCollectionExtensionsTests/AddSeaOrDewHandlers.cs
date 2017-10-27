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
        public void NoOptions_OnlyDefaultServicesRegistered()
        {
            var fakeCollection = new TestServiceCollection();
            fakeCollection.AddSeaOrDewHandlers();

            Assert.Collection(
                fakeCollection,
            s =>
            {
                Assert.Equal(s.ServiceType,typeof(QueryHandler));
            },
            s =>
            {
                Assert.Equal(s.ServiceType, typeof(CommandHandler));
            });
        }
    }
}