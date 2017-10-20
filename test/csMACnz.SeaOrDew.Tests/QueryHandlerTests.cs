using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace csMACnz.SeaOrDew.Tests
{
    public class QueryHandlerTests
    {
        private QueryHandler BuildSut()
        {
            var services = new ServiceCollection();

            return new QueryHandler(services.BuildServiceProvider());
        }

        private QueryHandler BuildSut<TCommand, TResult>(IQueryHandler<TCommand, TResult> instance)
        {
            var services = new ServiceCollection();
            services.AddSingleton<IQueryHandler<TCommand, TResult>>(instance);

            return new QueryHandler(services.BuildServiceProvider());
        }

        [Fact]
        public async Task Handle_WithoutContacts_WorksDirectly()
        {
            var sut = BuildSut<GetFooQuery, List<string>>(new GetFooQueryHandler());

            var result = await sut.Handle<GetFooQuery, List<string>>(new GetFooQuery());
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        private class GetFooQuery { }
        private class GetFooQueryHandler : IQueryHandler<GetFooQuery, List<string>>
        {
            public Task<List<string>> Handle(GetFooQuery query)
            {
                return Task.FromResult(new List<string>());
            }
        }

        [Fact]
        public async Task Handle_WithContracts_WorksDirectory()
        {
            var sut = BuildSut<GetBarQuery, GetBarQueryResult>(new GetBarQueryHandler());

            var result = await sut.Handle<GetBarQuery, GetBarQueryResult>(new GetBarQuery());
            Assert.NotNull(result);
            Assert.IsType<GetBarQueryResult>(result);
        }

        [Fact]
        public async Task Handle_WithContracts_WorksWithSpecifiedResult()
        {
            var sut = BuildSut<GetBarQuery, GetBarQueryResult>(new GetBarQueryHandler());

            var result = await sut.Handle<GetBarQueryResult>(new GetBarQuery());
            Assert.NotNull(result);
            Assert.IsType<GetBarQueryResult>(result);
        }

        [Fact]
        public async Task Handle_WithContracts_WorksImplicity()
        {
            var sut = BuildSut<GetBarQuery, GetBarQueryResult>(new GetBarQueryHandler());

            var result = await sut.Handle(new GetBarQuery());
            Assert.NotNull(result);
            Assert.IsType<GetBarQueryResult>(result);
        }

        private class GetBarQuery: IQuery<GetBarQueryResult> { }
        private class GetBarQueryResult { }
        private class GetBarQueryHandler : IQueryHandler<GetBarQuery, GetBarQueryResult>
        {
            public Task<GetBarQueryResult> Handle(GetBarQuery query)
            {
                return Task.FromResult(new GetBarQueryResult());
            }
        }

        [Fact]
        public async Task Handle_MissingQueryHandler_ThrowsException()
        {
            var sut = BuildSut();

            var exception = await Assert.ThrowsAsync<HandlerNotFoundException>(async () => await sut.Handle<UnregisteredQuery, UnregisteredQueryResult>(new UnregisteredQuery()));
            Assert.Equal(typeof(IQueryHandler<UnregisteredQuery, UnregisteredQueryResult>), exception.ExpectedType);
            Assert.Equal(
                $@"Could not resolve IQueryHandler<UnregisteredQuery, UnregisteredQueryResult> from the service provider. Please try one of the following:
* Check that you have registered the type with the IServiceProvider
* Make sure your Query matches the types expected by the QueryHandler
* Use the correct types when calling Handle
* Expected Query Type: {typeof(UnregisteredQuery).FullName}
* Expected Response Type: {typeof(UnregisteredQueryResult).FullName}",
                exception.Message);
        }

        public class UnregisteredQuery { }
        public class UnregisteredQueryResult { }
    }
}
