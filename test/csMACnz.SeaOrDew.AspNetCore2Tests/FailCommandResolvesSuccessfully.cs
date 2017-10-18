using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace csMACnz.SeaOrDew.AspNetCore2Tests
{
    public class FailCommandResolvesSuccessfully:GivenAnHttpContext
    {
        [Fact]
        public async Task Test()
        {
            var result = await Client.GetAsync("commandFail");

            Assert.Equal(HttpStatusCode.PreconditionFailed, result.StatusCode);
            Assert.Equal("failed command", await result.Content.ReadAsStringAsync());
        }
    }
}
