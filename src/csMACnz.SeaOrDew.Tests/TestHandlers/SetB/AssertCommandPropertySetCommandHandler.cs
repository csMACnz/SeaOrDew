using System.Threading.Tasks;

namespace csMACnz.SeaOrDew.Tests.TestHandlers.SetA
{
    public class AssertCommandPropertySetCommandHandler : ICommandHandler<AssertCommandPropertySetCommand>
    {
        public async Task<CommandResult<CommandError>> Handle(AssertCommandPropertySetCommand command)
        {
            await Task.Delay(0);
            if(command.ASetProperty)
            {
                return CommandResult<CommandError>.Success;
            }
            return new CommandError("Failed.");
        }
    }
    
    public class AssertCommandPropertySetCommand : ICommand
    {
        public bool ASetProperty { get; set; }
    }
}