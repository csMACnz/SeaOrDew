using System.Threading.Tasks;

namespace csMACnz.SeaOrDew.Tests.TestHandlers.SetB
{
    public class FailureTestCommandHandler : ICommandHandler<FailureTestCommand>
    {
        public async Task<CommandResult<CommandError>> Handle(FailureTestCommand command)
        {
            await Task.Delay(0);
            return new CommandError("This always fails.");
        }
    }
    
    public class FailureTestCommand : ICommand
    {
    }
}