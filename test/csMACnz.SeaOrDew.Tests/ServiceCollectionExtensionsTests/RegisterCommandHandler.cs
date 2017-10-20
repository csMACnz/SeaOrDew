using System;
using csMACnz.SeaOrDew.Tests.TestHandlers.SetA;
using Xunit;
using static csMACnz.SeaOrDew.Tests.TestHandlers.Stats;

namespace csMACnz.SeaOrDew.Tests.ServiceCollectionExtensionsTests
{
    public class RegisterCommandHandler
    {
        [Fact]
        public void InvalidType_ThrowsException()
        {
            var fakeCollection = new TestServiceCollection();
            Assert.Throws<NotSupportedException>(() => fakeCollection.RegisterCommandHandler<TestQueryHandler>());
        }

        [Fact]
        public void CommandRegistersCorrectly()
        {
            var fakeCollection = new TestServiceCollection();
            fakeCollection.RegisterCommandHandler<CustomTestCommandHandler>();

            Assert.Collection(fakeCollection,
                s =>
                {
                    Assert.Equal(typeof(ICustomCommandHandler<CustomTestCommand, CustomTestCommandResult>), s.ServiceType);
                    Assert.Equal(typeof(CustomTestCommandHandler), s.ImplementationType);
                });
        }
    }
}