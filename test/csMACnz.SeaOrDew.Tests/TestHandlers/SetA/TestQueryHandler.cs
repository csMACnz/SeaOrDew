using System.Net;
using System.Threading.Tasks;

namespace csMACnz.SeaOrDew.Tests.TestHandlers.SetA
{
    public class TestQueryHandler : IQueryHandler<TestQuery, TestQueryResult>
    {
        public Task<TestQueryResult> Handle(TestQuery command)
        {
            return Task.FromResult(new TestQueryResult());
        }
    }

    public class TestQuery : IQuery<TestQueryResult>
    {
    }

    public class TestQueryResult { }
}