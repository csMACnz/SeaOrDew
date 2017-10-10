using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace csMACnz.SeaOrDew
{
    public class CommandHandler
    {
        private readonly IServiceProvider _provider;

        public CommandHandler(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task<CommandResult<TError>> Handle<TCommand, TError>(TCommand command)
        {
            var service = _provider.GetRequiredService<ICommandHandler<TCommand, TError>>();
            return await service.Handle(command);
        }

        public Task<CommandResult<CommandError>> Handle<TCommand>(TCommand command)
        {
            return Handle<TCommand, CommandError>(command);
        }
    }
}
