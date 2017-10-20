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
            var service = _provider.GetService<IQueryHandler<TQuery, TResult>>() ?? throw NewHandlerNotFoundException<TQuery, TResult>();
            return await service.Handle(command);
        }

        private Exception NewHandlerNotFoundException<TQuery, TResult>()
        {
            var expectedType = typeof(IQueryHandler<TQuery, TResult>);
            var queryType = typeof(TQuery);
            var resultType = typeof(TResult);
            
            return new HandlerNotFoundException(
                expectedType,
                $@"Could not resolve IQueryHandler<{queryType.Name}, {resultType.Name}> from the service provider. Please try one of the following:
* Check that you have registerd the type with the IServiceProvider
* Make sure your Query matches the types expected by the QueryHandler
* Use the correct types when calling Handle
* Expected Query Type: {queryType.FullName}
* Expected Response Type: {resultType.FullName}");
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