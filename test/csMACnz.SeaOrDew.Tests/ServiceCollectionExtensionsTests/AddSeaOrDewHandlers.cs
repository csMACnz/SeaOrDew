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

            ServicesAssert.OnlyHandlerResolversRegistered(fakeCollection);
        }

        [Fact]
        public void NullOptions_CorrectlyAddsMainServices()
        {
            var fakeCollection = new TestServiceCollection();
            fakeCollection.AddSeaOrDewHandlers((SeaOrDewOptions)null);

            ServicesAssert.OnlyHandlerResolversRegistered(fakeCollection);
        }

        [Fact]
        public void PassOptions_CorrectlyAddsMainServices()
        {
            var fakeCollection = new TestServiceCollection();
            fakeCollection.AddSeaOrDewHandlers(new SeaOrDewOptions());

            ServicesAssert.OnlyHandlerResolversRegistered(fakeCollection);
        }

        [Fact]
        public void NullLambda_CorrectlyAddsMainServices()
        {
            var fakeCollection = new TestServiceCollection();
            fakeCollection.AddSeaOrDewHandlers((Action<SeaOrDewOptions>)null);

            ServicesAssert.OnlyHandlerResolversRegistered(fakeCollection);
        }

        [Fact]
        public void OptionsLambda_CorrectlyAddsMainServices()
        {
            var fakeCollection = new TestServiceCollection();
            fakeCollection.AddSeaOrDewHandlers(options =>
            {
            });

            ServicesAssert.OnlyHandlerResolversRegistered(fakeCollection);
        }

        [Fact]
        public void NoOptions_OnlyDefaultServicesRegistered()
        {
            var fakeCollection = new TestServiceCollection();
            fakeCollection.AddSeaOrDewHandlers();
            
            ServicesAssert.OnlyHandlerResolversRegistered(fakeCollection);
        }
    }
}