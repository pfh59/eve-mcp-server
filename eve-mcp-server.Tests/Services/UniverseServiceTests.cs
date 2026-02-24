using eve_mcp_server.Models;
using eve_mcp_server.Services;

namespace eve_mcp_server.Tests.Services;

public class UniverseServiceTests
{
    private readonly UniverseService _svc;
    private readonly MockHttpMessageHandler _handler;

    public UniverseServiceTests()
    {
        var (client, handler) = TestEsiClientFactory.Create();
        _handler = handler;
        _svc = new UniverseService(client);
    }

    [Fact]
    public async Task GetRegionsAsync_ReturnsRegionIds()
    {
        _handler.QueueJsonResponse(new[] { 10000001L, 10000002L, 10000003L });

        var result = await _svc.GetRegionsAsync();

        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetRegionAsync_ReturnsRegionDetails()
    {
        var region = new { region_id = 10000002, name = "The Forge", constellation_ids = new[] { 20000004L } };
        _handler.QueueJsonResponse(region);

        var result = await _svc.GetRegionAsync(10000002);

        Assert.NotNull(result);
        Assert.Equal("The Forge", result.Name);
    }

    [Fact]
    public async Task GetSystemAsync_ReturnsSystemDetails()
    {
        var system = new
        {
            system_id = 30000142,
            name = "Jita",
            security_status = 0.9459,
            constellation_id = 20000020,
            star_id = 40009081
        };
        _handler.QueueJsonResponse(system);

        var result = await _svc.GetSystemAsync(30000142);

        Assert.NotNull(result);
        Assert.Equal("Jita", result.Name);
        Assert.True(result.SecurityStatus > 0.9);
    }

    [Fact]
    public async Task GetTypeAsync_ReturnsTypeDetails()
    {
        var type = new
        {
            type_id = 34,
            name = "Tritanium",
            description = "The main building block in space structures.",
            volume = 0.01,
            group_id = 18
        };
        _handler.QueueJsonResponse(type);

        var result = await _svc.GetTypeAsync(34);

        Assert.NotNull(result);
        Assert.Equal("Tritanium", result.Name);
        Assert.Equal(0.01, result.Volume);
    }

    [Fact]
    public async Task GetFactionsAsync_ReturnsFactions()
    {
        var factions = new[]
        {
            new { faction_id = 500001, name = "Caldari State", description = "The Caldari State" },
            new { faction_id = 500002, name = "Minmatar Republic", description = "The Minmatar Republic" }
        };
        _handler.QueueJsonResponse(factions);

        var result = await _svc.GetFactionsAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetStationAsync_ReturnsStationDetails()
    {
        var station = new
        {
            station_id = 60003760,
            name = "Jita IV - Moon 4 - Caldari Navy Assembly Plant",
            system_id = 30000142,
            type_id = 52678,
            owner = 1000035
        };
        _handler.QueueJsonResponse(station);

        var result = await _svc.GetStationAsync(60003760);

        Assert.NotNull(result);
        Assert.Contains("Jita", result.Name);
    }

    [Fact]
    public async Task GetConstellationsAsync_ReturnsConstellationIds()
    {
        _handler.QueueJsonResponse(new[] { 20000001L, 20000002L });

        var result = await _svc.GetConstellationsAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetCategoriesAsync_ReturnsCategoryIds()
    {
        _handler.QueueJsonResponse(new[] { 1L, 2L, 3L, 4L });

        var result = await _svc.GetCategoriesAsync();

        Assert.NotNull(result);
        Assert.Equal(4, result.Count);
    }
}
