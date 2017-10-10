using System.Threading.Tasks;

namespace csMACnz.SeaOrDew
{
    public interface ICommandHandler<in TCommand, TError>
    {
        Task<CommandResult<TError>> Handle(TCommand command);
    }
}
