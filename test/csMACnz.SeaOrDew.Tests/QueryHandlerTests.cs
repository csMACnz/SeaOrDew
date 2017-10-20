using csMACnz.SeaOrDew.Tests.Fakes;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace csMACnz.SeaOrDew.Tests
{
    public class QueryHandlerTests
    {
        [Fact]
        public async Task Test()
        {
            var fakeprovider = new FakeServiceProvider();
            fakeprovider.Add<IQueryHandler<GetFooQuery, List<string>>>(new GetFooQueryHandler());

            var sut = new QueryHandler(fakeprovider);

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
        public async Task Handle_MissingQueryHandler_ThrowsException()
        {
            var fakeprovider = new FakeServiceProvider();
            var sut = new QueryHandler(fakeprovider);

            var exception = await Assert.ThrowsAsync<HandlerNotFoundException>(async () => await sut.Handle<UnregisteredQuery, UnregisteredQueryResult>(new UnregisteredQuery()));
            Assert.Equal(typeof(IQueryHandler<UnregisteredQuery, UnregisteredQueryResult>), exception.ExpectedType);
            Assert.Equal(
                $@"Could not resolve IQueryHandler<UnregisteredQuery, UnregisteredQueryResult> from the service provider. Please try one of the following:
* Check that you have registerd the type with the IServiceProvider
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
