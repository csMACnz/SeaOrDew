using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class QueryResolvesSuccessfully:GivenAnHttpContext
    {
        [Fact]
        public async Task Test()
        {
            var result = await Client.GetAsync("query");

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("query", await result.Content.ReadAsStringAsync());
        }
    }

    public class InvalidPathNotFound : GivenAnHttpContext
    {
        [Fact]
        public async Task Test()
        {
            var result = await Client.GetAsync("not/a/real/path");

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Not Found", await result.Content.ReadAsStringAsync());
        }
    }
}
