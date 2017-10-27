using System.Threading.Tasks;
using csMACnz.SeaOrDew.Tests.TestHandlers.SetA;
using Xunit;
using System.Net;
using csMACnz.SeaOrDew.Tests.TestHandlers.SetB;
using Microsoft.Extensions.DependencyInjection;

namespace csMACnz.SeaOrDew.Tests
{
    public class TestHandlersTests
    {
        private CommandHandler BuildCommandHandler<T>()
        {
            var services = new ServiceCollection();
            services.RegisterCommandHandler<T>();

            return new CommandHandler(services.BuildServiceProvider());
        }

        private QueryHandler BuildQueryHandler<T>()
        {
            var services = new ServiceCollection();
            services.RegisterQueryHandler<T>();

            return new QueryHandler(services.BuildServiceProvider());
        }

        [Fact]
        public async Task SetA_CustomTestCommandHandlerTest()
        {
            var sut = BuildCommandHandler<CustomTestCommandHandler>();

            var result = await sut.Handle(new CustomTestCommand());
            Assert.NotNull(result);
        }

        [Fact]
        public async Task SetA_HttpStatusTestCommandHandlerTest()
        {
            var sut = BuildCommandHandler<HttpStatusTestCommandHandler>();
            
            var result = await sut.Handle(new HttpStatusTestCommand());
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task SetA_TestCommandHandlerTest()
        {
            var sut = BuildCommandHandler<TestCommandHandler>();

            var result = await sut.Handle(new TestCommand());
            Assert.True(result.IsSuccess);
        }
        
        [Fact]
        public async Task SetA_TestQueryHandlerTest()
        {
            var sut = BuildQueryHandler<TestQueryHandler>();

            var result = await sut.Handle(new TestQuery());
            Assert.NotNull(result);
        }

        [Fact]
        public async Task SetB_AssertCommandPropertySetCommandHandler_SuccessMode()
        {
            var sut = BuildCommandHandler<AssertCommandPropertySetCommandHandler>();

            var result = await sut.Handle(new AssertCommandPropertySetCommand { ASetProperty = true });
            Assert.True(result.IsSuccess);
        }
        
        [Fact]
        public async Task SetB_AssertCommandPropertySetCommandHandler_FailMode()
        {
            var sut = BuildCommandHandler<AssertCommandPropertySetCommandHandler>();

            var result = await sut.Handle(new AssertCommandPropertySetCommand { ASetProperty = false });
            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Problem);
            Assert.Equal("Failed.", result.Problem.Message);
        }
        
        [Fact]
        public async Task SetB_FailureTestCommandHandlerTest()
        {
            var sut = BuildCommandHandler<FailureTestCommandHandler>();

            var result = await sut.Handle(new FailureTestCommand());
            Assert.False(result.IsSuccess);
        }
    }
}