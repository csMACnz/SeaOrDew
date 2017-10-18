using System.Net;
using System.Threading.Tasks;

namespace csMACnz.SeaOrDew.Tests.TestHandlers.SetA
{
    public class CustomTestCommandHandler : ICustomCommandHandler<CustomTestCommand, CustomTestCommandResult>
    {
        public Task<CustomTestCommandResult> Handle(CustomTestCommand command)
        {
            return Task.FromResult(new CustomTestCommandResult());
        }
    }

    public class CustomTestCommand : ICustomCommand<CustomTestCommandResult>
    {
    }

    public class CustomTestCommandResult { }
}