using Xunit;
using static csMACnz.SeaOrDew.Tests.TestHandlers.Stats;

namespace csMACnz.SeaOrDew.Tests.ServiceCollectionExtensionsTests
{
    public class LoadQueryHandlersFromAssemblyUnderNamespace
    {
        [Theory]
        [InlineData("csMACnz.SeaOrDew.Tests.TestHandlers", AssemblyQueryHandlerCount)]
        [InlineData("csMACnz.SeaOrDew.Tests.TestHandlers.SetA", SetAQueryCount)]
        [InlineData("csMACnz.SeaOrDew.Tests.TestHandlers.SetB", SetBQueryCount)]
        public void LoadQueryHandlersOnlyUnderNamespace_CorrectlyAddsHandlers(
            string namespacePrefix,
            int handlerCount)
        {
            var fakeCollection = new TestServiceCollection();
            fakeCollection.AddSeaOrDewHandlers(options =>
            {
                options.LoadQueryHandlersFromAssemblyUnderNamespace(TestHandlersAssembly, namespacePrefix);
            });

            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(CommandHandler));
            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(QueryHandler));
            Assert.Equal(handlerCount + 2, fakeCollection.Count);
        }
    }
}