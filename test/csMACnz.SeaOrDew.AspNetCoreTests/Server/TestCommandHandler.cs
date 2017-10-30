using System.Threading.Tasks;
using csMACnz.SeaOrDew;

namespace Tests.Server
{
    public class TestCommandHandler : ICommandHandler<TestCommand>
    {
        public async Task<CommandResult<CommandError>> Handle(TestCommand command)
        {
            await Task.Delay(0);
            if (command.Pass)
            {
                return Handler.Success;
            }
            else
            {
                return new CommandError("This always fails.");
            }
        }
    }

    public class TestCommand : ICommand
    {
        public TestCommand(bool pass)
        {
            Pass = pass;
        }
        public bool Pass { get; }
    }
}