using Xunit;
using static csMACnz.SeaOrDew.Tests.TestHandlers.Stats;

namespace csMACnz.SeaOrDew.Tests.ServiceCollectionExtensionsTests
{
    public class LoadCommandHandlersFromAssemblyUnderNamespace
    {
        [Theory]
        [InlineData("csMACnz.SeaOrDew.Tests.TestHandlers", AssemblyCommandHandlerCount)]
        [InlineData("csMACnz.SeaOrDew.Tests.TestHandlers.SetA", SetACommandCount)]
        [InlineData("csMACnz.SeaOrDew.Tests.TestHandlers.SetB", SetBCommandCount)]
        public void CorrectlyAddsHandlers(
            string namespacePrefix,
            int handlerCount)
        {
            var fakeCollection = new TestServiceCollection();
            fakeCollection.AddSeaOrDewHandlers(options =>
            {
                options.LoadCommandHandlersFromAssemblyUnderNamespace(TestHandlersAssembly, namespacePrefix);
            });

            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(CommandHandler));
            Assert.Contains(fakeCollection, s => s.ServiceType == typeof(QueryHandler));
            Assert.Equal(handlerCount + 2, fakeCollection.Count);
        }
    }
}