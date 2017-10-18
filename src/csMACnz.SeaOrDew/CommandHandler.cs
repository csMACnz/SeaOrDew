using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace csMACnz.SeaOrDew
{
    public class CommandHandler
    {
        private readonly IServiceProvider _provider;

        public CommandHandler(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task<TResult> Handle<TCommand, TResult>(TCommand command)
        {
            var service = _provider.GetRequiredService<ICustomCommandHandler<TCommand, TResult>>();
            return await service.Handle(command);
        }
    }

    public static class CommandHandlerExtensions
    {
        public static Task<CommandResult<CommandError>> Handle<TCommand>(this CommandHandler handler, TCommand command)
        {
            return handler.Handle<TCommand, CommandResult<CommandError>>(command);
        }
        
        public static Task<TResult> Handle<TCommand, TResult>(this CommandHandler handler, TCommand command)
        where TCommand: ICustomCommand<TResult>
        {
            return handler.Handle<TCommand, TResult>(command);
        }
    }
}
