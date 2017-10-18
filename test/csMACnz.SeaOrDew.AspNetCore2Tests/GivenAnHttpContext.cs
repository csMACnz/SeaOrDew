using csMACnz.SeaOrDew.AspNetCore2Tests.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;

namespace csMACnz.SeaOrDew.AspNetCore2Tests
{
    public class GivenAnHttpContext
    {
        protected HttpClient Client;
        
        public GivenAnHttpContext()
        {
            var testServer = BuildTestServer();

            Client = testServer.CreateClient();
        }

        private TestServer BuildTestServer()
        {
            var webHostBuilder = new WebHostBuilder()
                .UseStartup<Startup>()
                .UseEnvironment("Development");

            var testServer = new TestServer(webHostBuilder);
            return testServer;
        }
    }
}
