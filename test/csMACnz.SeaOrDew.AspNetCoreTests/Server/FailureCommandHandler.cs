using System.Threading.Tasks;
using csMACnz.SeaOrDew;

namespace Tests.Server
{
    public class FailureCommandHandler : ICommandHandler<FailureCommand>
    {
        public async Task<CommandResult<CommandError>> Handle(FailureCommand command)
        {
            await Task.Delay(0);
            return new CommandError("This always fails.");
        }
    }
    
    public class FailureCommand : ICommand
    {
    }
}