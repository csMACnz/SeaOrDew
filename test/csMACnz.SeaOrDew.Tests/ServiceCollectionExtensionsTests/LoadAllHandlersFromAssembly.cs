using Xunit;
using static csMACnz.SeaOrDew.Tests.TestHandlers.Stats;

namespace csMACnz.SeaOrDew.Tests.ServiceCollectionExtensionsTests
{
    public class LoadAllHandlersFromAssembly
    {
        [Fact]
        public void LoadAllFromAssembly_CorrectlyAddsHandlers()
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