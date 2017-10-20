using Xunit;
using static csMACnz.SeaOrDew.Tests.TestHandlers.Stats;

namespace csMACnz.SeaOrDew.Tests.ServiceCollectionExtensionsTests
{
    public class LoadAssemblyHandlersUnderNamespace
    {
        [Theory]
        [InlineData("csMACnz.SeaOrDew.Tests.TestHandlers", AssemblyHandlerCount)]
        [InlineData("csMACnz.SeaOrDew.Tests.TestHandlers.SetA", SetACount)]
        [InlineData("csMACnz.SeaOrDew.Tests.TestHandlers.SetB", SetBCount)]
        public void LoadAssemblyHandlersUnderNamespace_CorrectlyAddsHandlers(
            string namespacePrefix,
            int handlerCount)
        {
            var fakeCollection = new TestServiceCollection();
            fakeCollection.AddSeaOrDewHandlers(options =>
            {
                options.LoadAssemblyHandlersUnderNamespace(TestHandlersAssembly, namespacePrefix);
            });

            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(CommandHandler));
            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(QueryHandler));
            Assert.Equal(handlerCount + 2, fakeCollection.Count);
        }
    }
}