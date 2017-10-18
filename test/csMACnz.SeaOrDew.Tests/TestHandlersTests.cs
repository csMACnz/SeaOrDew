using System.Threading.Tasks;
using csMACnz.SeaOrDew.Tests.Fakes;
using csMACnz.SeaOrDew.Tests.TestHandlers.SetA;
using Xunit;
using System.Net;

namespace csMACnz.SeaOrDew.Tests
{
    public class TestHandlersTests
    {
        [Fact]
        public async Task SetA_CustomTestCommandHandlerTest()
        {
            var instance = new CustomTestCommandHandler();
            var fakeProvider = new FakeServiceProvider();
            fakeProvider.AddHandler(instance);

            var sut = new CommandHandler(fakeProvider);

            CustomTestCommandResult result = await sut.Handle(new CustomTestCommand());
            Assert.NotNull(result);
        }

        [Fact]
        public async Task SetA_HttpStatusTestCommandHandlerTest()
        {
            var instance = new HttpStatusTestCommandHandler();
            var fakeProvider = new FakeServiceProvider();
            fakeProvider.AddHandler(instance);

            var sut = new CommandHandler(fakeProvider);

            CommandResult<HttpStatusCode> result = await sut.Handle(new HttpStatusTestCommand());
            Assert.True(result.IsSuccess);
        }
    }
}