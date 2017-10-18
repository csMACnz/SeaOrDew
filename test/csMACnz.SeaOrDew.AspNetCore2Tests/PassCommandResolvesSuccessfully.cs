using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace csMACnz.SeaOrDew.AspNetCore2Tests
{
    public class PassCommandResolvesSuccessfully:GivenAnHttpContext
    {
        [Fact]
        public async Task Test()
        {
            var result = await Client.GetAsync("commandPass");

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("command", await result.Content.ReadAsStringAsync());
        }
    }
}
