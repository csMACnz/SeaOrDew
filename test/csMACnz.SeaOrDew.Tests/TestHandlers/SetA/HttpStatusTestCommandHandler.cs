using System.Net;
using System.Threading.Tasks;

namespace csMACnz.SeaOrDew.Tests.TestHandlers.SetA
{
    public class HttpStatusTestCommandHandler : ICommandHandler<HttpStatusTestCommand, HttpStatusCode>
    {
        public Task<CommandResult<HttpStatusCode>> Handle(HttpStatusTestCommand command)
        {
            return Task.FromResult(CommandResult<HttpStatusCode>.Success);
        }
    }
    
    public class HttpStatusTestCommand : ICommand<HttpStatusCode>
    {
    }
}