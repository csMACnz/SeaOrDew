using Tests.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;

namespace Tests
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
