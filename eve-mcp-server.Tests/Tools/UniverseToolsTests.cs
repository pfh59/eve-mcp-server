using eve_mcp_server.Services;
using eve_mcp_server.Tools;

namespace eve_mcp_server.Tests.Tools;

public class UniverseToolsTests
{
    private readonly UniverseService _svc;
    private readonly MockHttpMessageHandler _handler;

    public UniverseToolsTests()
    {
        var (client, handler) = TestEsiClientFactory.Create();
        _handler = handler;
        _svc = new UniverseService(client);
    }

    [Fact]
    public async Task GetRegions_ReturnsJsonWithRegionIds()
    {
        _handler.QueueJsonResponse(new[] { 10000001L, 10000002L });

        var result = await UniverseTools.GetRegions(_svc, CancellationToken.None);

        Assert.Contains("10000001", result);
        Assert.Contains("10000002", result);
    }

    [Fact]
    public async Task GetRegion_WhenFound_ReturnsJsonDetails()
    {
        _handler.QueueJsonResponse(new { region_id = 10000002, name = "The Forge" });

        var result = await UniverseTools.GetRegion(_svc, 10000002, CancellationToken.None);

        Assert.Contains("The Forge", result);
    }

    [Fact]
    public async Task GetRegion_WhenNotFound_ReturnsNotFoundMessage()
    {
        _handler.Queue404();

        var result = await UniverseTools.GetRegion(_svc, 999999, CancellationToken.None);

        Assert.Equal("Region not found.", result);
    }

    [Fact]
    public async Task GetSolarSystem_ReturnsSystemInfo()
    {
        _handler.QueueJsonResponse(new { system_id = 30000142, name = "Jita", security_status = 0.9459 });

        var result = await UniverseTools.GetSolarSystem(_svc, 30000142, CancellationToken.None);

        Assert.Contains("Jita", result);
    }

    [Fact]
    public async Task GetType_ReturnsTypeInfo()
    {
        _handler.QueueJsonResponse(new { type_id = 34, name = "Tritanium" });

        var result = await UniverseTools.GetType(_svc, 34, CancellationToken.None);

        Assert.Contains("Tritanium", result);
    }

    [Fact]
    public async Task GetFactions_ReturnsFactionList()
    {
        _handler.QueueJsonResponse(new[]
        {
            new { faction_id = 500001, name = "Caldari State" },
            new { faction_id = 500002, name = "Minmatar Republic" }
        });

        var result = await UniverseTools.GetFactions(_svc, CancellationToken.None);

        Assert.Contains("Caldari", result);
        Assert.Contains("Minmatar", result);
    }
}
