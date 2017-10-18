using System.Threading.Tasks;

namespace csMACnz.SeaOrDew.AspNetCore2Tests.Server
{
    public class TestQueryHandler : IQueryHandler<TestQuery, TestResult>
    {
        public async Task<TestResult> Handle(TestQuery query)
        {
            await Task.Delay(0);
            return new TestResult();
        }
    }

    public class TestQuery: IQuery<TestResult> { }

    public class TestResult { }
}