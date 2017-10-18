using System.Threading.Tasks;

namespace csMACnz.SeaOrDew
{
    public interface ICustomCommandHandler<TCommand, TResult>
    {
        Task<TResult> Handle(TCommand command);
    }

    public interface ICommandHandler<TCommand, TError> : ICustomCommandHandler<TCommand, CommandResult<TError>>
    {
    }

    public interface ICommandHandler<TCommand> : ICommandHandler<TCommand, CommandError>
    {
    }

    public interface ICustomCommand<TResult>
    {

    }
    public interface ICommand<TError> : ICustomCommand<CommandResult<TError>>
    {

    }
    public interface ICommand : ICommand<CommandError>
    {

    }
}