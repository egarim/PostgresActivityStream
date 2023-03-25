using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace Tests.Infrastructure
{
    public class TestClientFactory : IHttpClientFactory
    {
        TestServer _TestServer;

        public TestClientFactory(TestServer TestServer)
        {
            _TestServer = TestServer;
        }

        public TimeSpan Timeout { get; set; }

        Dictionary<string, HttpClient> Clients = new Dictionary<string, HttpClient>();
        public HttpClient CreateClient(string name)
        {
            if (Clients.ContainsKey(name))
                return Clients[name];

            HttpClient NewClient = _TestServer.CreateClient();
            Clients.Add(name, NewClient);

            return NewClient;
        }

        public void Dispose()
        {
            _TestServer.Dispose();
        }
    }
}