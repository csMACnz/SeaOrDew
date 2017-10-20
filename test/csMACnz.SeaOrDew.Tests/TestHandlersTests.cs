using System.Threading.Tasks;
using csMACnz.SeaOrDew.Tests.Fakes;
using csMACnz.SeaOrDew.Tests.TestHandlers.SetA;
using Xunit;
using System.Net;
using csMACnz.SeaOrDew.Tests.TestHandlers.SetB;

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

            var result = await sut.Handle(new CustomTestCommand());
            Assert.NotNull(result);
        }

        [Fact]
        public async Task SetA_HttpStatusTestCommandHandlerTest()
        {
            var instance = new HttpStatusTestCommandHandler();
            var fakeProvider = new FakeServiceProvider();
            fakeProvider.AddHandler(instance);

            var sut = new CommandHandler(fakeProvider);

            var result = await sut.Handle(new HttpStatusTestCommand());
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task SetA_TestCommandHandlerTest()
        {
            var instance = new TestCommandHandler();
            var fakeProvider = new FakeServiceProvider();
            fakeProvider.AddHandler(instance);

            var sut = new CommandHandler(fakeProvider);

            var result = await sut.Handle(new TestCommand());
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task SetB_AssertCommandPropertySetCommandHandlerTest()
        {
            var instance = new AssertCommandPropertySetCommandHandler();
            var fakeProvider = new FakeServiceProvider();
            fakeProvider.AddHandler(instance);

            var sut = new CommandHandler(fakeProvider);

            var result = await sut.Handle(new AssertCommandPropertySetCommand { ASetProperty = true });
            Assert.True(result.IsSuccess);
        }
        
        [Fact]
        public async Task SetB_FailureTestCommandHandlerTest()
        {
            var instance = new FailureTestCommandHandler();
            var fakeProvider = new FakeServiceProvider();
            fakeProvider.AddHandler(instance);

            var sut = new CommandHandler(fakeProvider);

            var result = await sut.Handle(new FailureTestCommand());
            Assert.False(result.IsSuccess);
        }
    }
}