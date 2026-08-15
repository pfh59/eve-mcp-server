using eve_mcp_server.Esi;
using Microsoft.Extensions.Logging.Abstractions;

namespace eve_mcp_server.Tests;

/// <summary>
/// Factory to create EsiClient instances backed by MockHttpMessageHandler for testing.
/// </summary>
public static class TestEsiClientFactory
{
    public static (EsiClient Client, MockHttpMessageHandler Handler) Create()
    {
        var handler = new MockHttpMessageHandler();
        var httpClient = new HttpClient(handler);
        var options = new EsiClientOptions
        {
            BaseUrl = "https://esi.evetech.net",
            UserAgent = "eve-mcp-server-tests/1.0.0",
            Datasource = "tranquility"
        };
        var logger = NullLogger<EsiClient>.Instance;
        var client = new EsiClient(httpClient, options, logger);
        return (client, handler);
    }
}
