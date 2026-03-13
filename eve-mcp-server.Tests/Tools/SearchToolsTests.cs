using eve_mcp_server.Services;
using eve_mcp_server.Tools;

namespace eve_mcp_server.Tests.Tools;

public class SearchToolsTests
{
    private readonly SearchService _svc;
    private readonly MockHttpMessageHandler _handler;

    public SearchToolsTests()
    {
        var (client, handler) = TestEsiClientFactory.Create();
        _handler = handler;
        _svc = new SearchService(client);
    }

    [Fact]
    public async Task ResolveNamesToIds_SplitsCommaDelimitedNames()
    {
        _handler.QueueJsonResponse(new
        {
            systems = new[] { new { id = 30000142, name = "Jita" } }
        });

        var result = await SearchTools.ResolveNamesToIds(_svc, "Jita, Amarr", CancellationToken.None);

        Assert.NotEqual("No matches found.", result);
        Assert.Equal(HttpMethod.Post, _handler.LastRequest.Method);
    }

    [Fact]
    public async Task ResolveIdsToNames_SplitsCommaDelimitedIds()
    {
        _handler.QueueJsonResponse(new[]
        {
            new { id = 30000142, name = "Jita", category = "solar_system" },
            new { id = 34, name = "Tritanium", category = "inventory_type" }
        });

        var result = await SearchTools.ResolveIdsToNames(_svc, "30000142,34", CancellationToken.None);

        Assert.Contains("Jita", result);
        Assert.Contains("Tritanium", result);
    }

    [Fact]
    public async Task ResolveIdsToNames_InvalidId_ReturnsErrorMessage()
    {
        var result = await SearchTools.ResolveIdsToNames(_svc, "30000142,notanumber,34", CancellationToken.None);

        Assert.Contains("Invalid ID value: 'notanumber'", result);
    }
}
