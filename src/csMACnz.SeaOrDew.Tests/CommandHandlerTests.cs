using csMACnz.SeaOrDew.Tests.Fakes;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace csMACnz.SeaOrDew.Tests
{
    public class CommandHandlerTests
    {
        [Fact]
        public async Task TestCommandHandler_Success()
        {
            var fakeprovider = new FakeServiceProvider();
            fakeprovider.Add<ICommandHandler<DoFizzBuzzCommand, HttpStatusCode>>(new DoFizzBuzzCommandHandler());

            var sut = new CommandHandler(fakeprovider);

            var result = await sut.Handle<DoFizzBuzzCommand, HttpStatusCode>(new DoFizzBuzzCommand());
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(default(HttpStatusCode), result.Problem);
        }

        private class DoFizzBuzzCommand { }
        private class DoFizzBuzzCommandHandler : ICommandHandler<DoFizzBuzzCommand, HttpStatusCode>
        {
            public Task<CommandResult<HttpStatusCode>> Handle(DoFizzBuzzCommand command)
            {
                return Task.FromResult(CommandResult<HttpStatusCode>.Success);
            }
        }

        [Fact]
        public async Task TestCommandHandler_Failure()
        {
            var fakeprovider = new FakeServiceProvider();
            fakeprovider.Add<ICommandHandler<FailToWorkCommand, HttpStatusCode>>(new FailToWorkCommandHandler());

            var sut = new CommandHandler(fakeprovider);

            var result = await sut.Handle<FailToWorkCommand, HttpStatusCode>(new FailToWorkCommand());
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Problem);
        }

        private class FailToWorkCommand { }
        private class FailToWorkCommandHandler : ICommandHandler<FailToWorkCommand, HttpStatusCode>
        {
            public async Task<CommandResult<HttpStatusCode>> Handle(FailToWorkCommand command)
            {
                await Task.Delay(0);
                return HttpStatusCode.InternalServerError;
            }
        }

        [Fact]
        public async Task CommandHandlerOfCommandError_Success()
        {
            var fakeprovider = new FakeServiceProvider();
            fakeprovider.Add<ICommandHandler<DoMakeWidgetCommand, CommandError>>(new DoMakeWidgetCommandHandler());

            var sut = new CommandHandler(fakeprovider);

            var result = await sut.Handle(new DoMakeWidgetCommand());
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(default(CommandError), result.Problem);
        }

        private class DoMakeWidgetCommand { }
        private class DoMakeWidgetCommandHandler : ICommandHandler<DoMakeWidgetCommand,CommandError>
        {
            public Task<CommandResult<CommandError>> Handle(DoMakeWidgetCommand command)
            {
                return Task.FromResult(CommandResult<CommandError>.Success);
            }
        }

        [Fact]
        public async Task CommandHandlerOfCommandError_Failure()
        {
            var fakeprovider = new FakeServiceProvider();
            fakeprovider.Add<ICommandHandler<FailToWorkWithCommandErrorCommand, HttpStatusCode>>(new FailToWorkWithCommandErrorCommandHandler());

            var sut = new CommandHandler(fakeprovider);

            var result = await sut.Handle<FailToWorkWithCommandErrorCommand, HttpStatusCode>(new FailToWorkWithCommandErrorCommand());
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Problem);
        }

        private class FailToWorkWithCommandErrorCommand { }
        private class FailToWorkWithCommandErrorCommandHandler : ICommandHandler<FailToWorkWithCommandErrorCommand, HttpStatusCode>
        {
            public async Task<CommandResult<HttpStatusCode>> Handle(FailToWorkWithCommandErrorCommand command)
            {
                await Task.Delay(0);
                return HttpStatusCode.InternalServerError;
            }
        }

    }
}
