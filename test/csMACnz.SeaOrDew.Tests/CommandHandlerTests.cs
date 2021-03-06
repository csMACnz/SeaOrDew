﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace csMACnz.SeaOrDew.Tests
{
    public class CommandHandlerTests
    {
        private CommandHandler BuildSut()
        {
            var services = new ServiceCollection();

            return new CommandHandler(services.BuildServiceProvider());
        }

        private CommandHandler BuildSut<TCommand, TResult>(ICustomCommandHandler<TCommand, TResult> instance)
        {
            var services = new ServiceCollection();
            services.AddSingleton<ICustomCommandHandler<TCommand, TResult>>(instance);

            return new CommandHandler(services.BuildServiceProvider());
        }

        [Fact]
        public async Task Handle_SuccessfulCommand_Success()
        {
            var sut = BuildSut<DoFizzBuzzCommand, CommandResult<HttpStatusCode>>(new DoFizzBuzzCommandHandler());

            var result = await sut.Handle<DoFizzBuzzCommand, CommandResult<HttpStatusCode>>(new DoFizzBuzzCommand());
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
        public async Task Handle_FailureCommand_Fails()
        {
            var sut = BuildSut<FailToWorkCommand, CommandResult<HttpStatusCode>>(new FailToWorkCommandHandler());

            var result = await sut.Handle<FailToWorkCommand, CommandResult<HttpStatusCode>>(new FailToWorkCommand());
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
        public async Task Handle_CommandWithCustomErrorType_Success()
        {
            var sut = BuildSut<DoMakeWidgetCommand, CommandResult<CommandError>>(new DoMakeWidgetCommandHandler());

            var result = await sut.HandleDefault(new DoMakeWidgetCommand());
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(default(CommandError), result.Problem);
        }

        private class DoMakeWidgetCommand { }
        private class DoMakeWidgetCommandHandler : ICommandHandler<DoMakeWidgetCommand, CommandError>
        {
            public Task<CommandResult<CommandError>> Handle(DoMakeWidgetCommand command)
            {
                return Task.FromResult(CommandResult<CommandError>.Success);
            }
        }

        [Fact]
        public async Task Handle_FailureCommandWithCustomErrorType_Failure()
        {
            var sut = BuildSut<FailToWorkWithCommandErrorCommand, CommandResult<HttpStatusCode>>(new FailToWorkWithCommandErrorCommandHandler());

            var result = await sut.Handle<FailToWorkWithCommandErrorCommand, CommandResult<HttpStatusCode>>(new FailToWorkWithCommandErrorCommand());
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

        [Fact]
        public async Task Handle_MissingCustomCommandHandler_ThrowsException()
        {
            var sut = BuildSut();

            var exception = await Assert.ThrowsAsync<HandlerNotFoundException>(async () => await sut.Handle<UnregisteredCommand, CustomResult>(new UnregisteredCommand()));
            Assert.Equal(typeof(ICustomCommandHandler<UnregisteredCommand, CustomResult>), exception.ExpectedType);
            Assert.Equal(
                $@"Could not resolve ICustomCommandHandler<UnregisteredCommand, CustomResult> from the service provider. Please try one of the following:
* Check that you have registered the type with the IServiceProvider
* Make sure your Command matches the types expected by the command handler
* Use the correct types when calling Handle
* Expected Command Type: {typeof(UnregisteredCommand).FullName}
* Expected Response Type: {typeof(CustomResult).FullName}",
                exception.Message);
        }

        [Fact]
        public async Task Handle_MissingCustomErrorHandler_ThrowsException()
        {
            var sut = BuildSut();

            var exception = await Assert.ThrowsAsync<HandlerNotFoundException>(async () => await sut.Handle<UnregisteredCommand, CommandResult<HttpStatusCode>>(new UnregisteredCommand()));
            Assert.Equal(typeof(ICustomCommandHandler<UnregisteredCommand, CommandResult<HttpStatusCode>>), exception.ExpectedType);
            Assert.Equal(
                $@"Could not resolve ICommandHandler<UnregisteredCommand, HttpStatusCode> from the service provider. Please try one of the following:
* Check that you have registered the type with the IServiceProvider
* Make sure your Command matches the types expected by the command handler
* Use the correct types when calling Handle
* Expected Command Type: {typeof(UnregisteredCommand).FullName}
* Expected Response Type: {typeof(CommandResult<HttpStatusCode>).FullName}",
                exception.Message);
        }

        [Fact]
        public async Task Handle_MissingHandler_ThrowsException()
        {
            var sut = BuildSut();

            var exception = await Assert.ThrowsAsync<HandlerNotFoundException>(async () => await sut.Handle<UnregisteredCommand, CommandResult<CommandError>>(new UnregisteredCommand()));
            Assert.Equal(typeof(ICustomCommandHandler<UnregisteredCommand, CommandResult<CommandError>>), exception.ExpectedType);
            Assert.Equal(
                $@"Could not resolve ICommandHandler<UnregisteredCommand> from the service provider. Please try one of the following:
* Check that you have registered the type with the IServiceProvider
* Make sure your Command matches the types expected by the command handler
* Use the correct types when calling Handle
* Expected Command Type: {typeof(UnregisteredCommand).FullName}
* Expected Response Type: {typeof(CommandResult<CommandError>).FullName}",
                exception.Message);
        }

        private class UnregisteredCommand { }
        private class CustomResult { }
    }
}
