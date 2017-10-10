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
    }
}
