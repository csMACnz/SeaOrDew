using System.Threading.Tasks;
using csMACnz.SeaOrDew;

namespace Tests.Server
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