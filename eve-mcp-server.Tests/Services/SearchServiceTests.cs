using eve_mcp_server.Models;
using eve_mcp_server.Services;

namespace eve_mcp_server.Tests.Services;

public class SearchServiceTests
{
    private readonly SearchService _svc;
    private readonly MockHttpMessageHandler _handler;

    public SearchServiceTests()
    {
        var (client, handler) = TestEsiClientFactory.Create();
        _handler = handler;
        _svc = new SearchService(client);
    }

    [Fact]
    public async Task ResolveNamesToIdsAsync_PostsNamesAndReturnsResult()
    {
        var idsResult = new
        {
            systems = new[] { new { id = 30000142, name = "Jita" } },
            inventory_types = new[] { new { id = 34, name = "Tritanium" } }
        };
        _handler.QueueJsonResponse(idsResult);

        var result = await _svc.ResolveNamesToIdsAsync(new List<string> { "Jita", "Tritanium" });

        Assert.NotNull(result);
        Assert.Equal(HttpMethod.Post, _handler.LastRequest.Method);
        Assert.Contains("/universe/ids/", _handler.LastRequest.RequestUri!.ToString());
    }

    [Fact]
    public async Task ResolveIdsToNamesAsync_PostsIdsAndReturnsNames()
    {
        var names = new[]
        {
            new { id = 30000142, name = "Jita", category = "solar_system" },
            new { id = 34, name = "Tritanium", category = "inventory_type" }
        };
        _handler.QueueJsonResponse(names);

        var result = await _svc.ResolveIdsToNamesAsync(new List<long> { 30000142, 34 });

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(HttpMethod.Post, _handler.LastRequest.Method);
    }
}
