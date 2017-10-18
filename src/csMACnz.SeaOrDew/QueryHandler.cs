using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

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

    public static class QueryHandlerExtensions
    {
        public static Task<TResult> Handle<TResult>(this QueryHandler handler, IQuery<TResult> command)
        {
            var queryType = command.GetType();
            var resultType = typeof(TResult);
            var handleMethod = typeof(QueryHandler).GetTypeInfo().GetDeclaredMethod(nameof(QueryHandler.Handle)).MakeGenericMethod(queryType, resultType);
            return (Task<TResult>)handleMethod.Invoke(handler, new[] { command });
        }
    }
}