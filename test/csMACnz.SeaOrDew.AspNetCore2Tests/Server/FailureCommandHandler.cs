using System.Threading.Tasks;

namespace csMACnz.SeaOrDew.AspNetCore2Tests.Server
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