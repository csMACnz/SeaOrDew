using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

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
        public static Task<CommandResult<CommandError>> HandleDefault<TCommand>(this CommandHandler handler, TCommand command)
        {
            return handler.Handle<TCommand, CommandResult<CommandError>>(command);
        }
        
        public static Task<TResult> Handle<TResult>(this CommandHandler handler, ICustomCommand<TResult> command)
        {
            var queryType = command.GetType();
            var resultType = typeof(TResult);
            var handleMethod = typeof(CommandHandler).GetTypeInfo().GetDeclaredMethod(nameof(CommandHandler.Handle)).MakeGenericMethod(queryType, resultType);
            return (Task<TResult>)handleMethod.Invoke(handler, new[] { command });
        }
    }
}
