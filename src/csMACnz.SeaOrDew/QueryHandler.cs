using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace csMACnz.SeaOrDew
{
    public class QueryHandler
    {
        private readonly IServiceProvider _provider;

        public QueryHandler(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task<TResult> Handle<TQuery, TResult>(TQuery command)
        {
            var service = _provider.GetRequiredService<IQueryHandler<TQuery, TResult>>();
            return await service.Handle(command);
        }
    }
}