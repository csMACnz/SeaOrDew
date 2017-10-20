using System;
using csMACnz.SeaOrDew.Tests.TestHandlers.SetA;
using Xunit;
using static csMACnz.SeaOrDew.Tests.TestHandlers.Stats;

namespace csMACnz.SeaOrDew.Tests.ServiceCollectionExtensionsTests
{
    public class RegisterQueryHandler
    {
        [Fact]
        public void InvalidType_ThrowsException()
        {
            var fakeCollection = new TestServiceCollection();
            Assert.Throws<NotSupportedException>(() => fakeCollection.RegisterQueryHandler<TestCommandHandler>());
        }

        [Fact]
        public void QueryRegistersCorrectly()
        {
            var fakeCollection = new TestServiceCollection();
            fakeCollection.RegisterQueryHandler<TestQueryHandler>();

            Assert.Collection(fakeCollection,
                s =>
                {
                    Assert.Equal(typeof(IQueryHandler<TestQuery, TestQueryResult>), s.ServiceType);
                    Assert.Equal(typeof(TestQueryHandler), s.ImplementationType);
                });
        }
    }
}