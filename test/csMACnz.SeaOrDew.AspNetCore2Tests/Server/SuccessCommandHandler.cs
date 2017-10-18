using System.Threading.Tasks;

namespace csMACnz.SeaOrDew.AspNetCore2Tests.Server
{
    public class SuccessCommandHandler : ICommandHandler<SuccessCommand>
    {
        public async Task<CommandResult<CommandError>> Handle(SuccessCommand command)
        {
            await Task.Delay(0);
            return Handler.Success;
        }
    }
    
    public class SuccessCommand : ICommand
    {
    }
}