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
            var service = _provider.GetService<ICustomCommandHandler<TCommand, TResult>>() ?? throw NewHandlerNotFoundException<TCommand, TResult>();
            
            return await service.Handle(command);
        }

        private Exception NewHandlerNotFoundException<TCommand, TResult>()
        {
            var expectedType = typeof(ICustomCommandHandler<TCommand, TResult>);
            var commandType = typeof(TCommand);
            var resultType = typeof(TResult);
            var handlerInterface = $"ICustomCommandHandler<{commandType.Name}, {resultType.Name}>";
            if(resultType == typeof(CommandResult<CommandError>))
            {
                handlerInterface = $"ICommandHandler<{commandType.Name}>";
            }
            else if (resultType.GetTypeInfo().IsConcreteInstanceOfGenericTypeDefinition(typeof(CommandResult<>)))
            {
                var errorType = resultType.GenericTypeArguments[0];
                handlerInterface = $"ICommandHandler<{commandType.Name}, {errorType.Name}>";
            }
            return new HandlerNotFoundException(
                expectedType,
                $@"Could not resolve {handlerInterface} from the service provider. Please try one of the following:
* Check that you have registerd the type with the IServiceProvider
* Make sure your Command matches the types expected by the command handler
* Use the correct types when calling Handle
* Expected Command Type: {commandType.FullName}
* Expected Response Type: {resultType.FullName}");
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
