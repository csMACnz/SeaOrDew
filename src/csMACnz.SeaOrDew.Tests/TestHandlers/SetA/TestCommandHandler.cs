using System.Threading.Tasks;

namespace csMACnz.SeaOrDew.Tests.TestHandlers.SetA
{
    public class TestCommandHandler : ICommandHandler<TestCommand>
    {
        public Task<CommandResult<CommandError>> Handle(TestCommand command)
        {
            return Task.FromResult(CommandResult<CommandError>.Success);
        }
    }
    
    public class TestCommand : ICommand
    {
    }
}