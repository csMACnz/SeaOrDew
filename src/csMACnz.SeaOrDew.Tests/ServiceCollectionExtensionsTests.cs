using Xunit;

namespace csMACnz.SeaOrDew.Tests
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddHandlerExtension_CorrectlyAddsMainServices()
        {
            var fakeCollection = new TestServiceCollection();
            fakeCollection.AddSeaOrDewHandlers();

            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(CommandHandler));
            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(QueryHandler));
        }
    }
}