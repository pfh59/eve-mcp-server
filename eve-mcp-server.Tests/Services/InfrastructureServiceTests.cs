using eve_mcp_server.Models;
using eve_mcp_server.Services;

namespace eve_mcp_server.Tests.Services;

public class InfrastructureServiceTests
{
    private readonly InfrastructureService _svc;
    private readonly MockHttpMessageHandler _handler;

    public InfrastructureServiceTests()
    {
        var (client, handler) = TestEsiClientFactory.Create();
        _handler = handler;
        _svc = new InfrastructureService(client);
    }

    [Fact]
    public async Task GetRouteAsync_ReturnsSystemIds()
    {
        _handler.QueueJsonResponse(new[] { 30000142L, 30000144L, 30000148L });

        var result = await _svc.GetRouteAsync(30000142, 30000148);

        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.Equal(30000142L, result[0]);
    }

    [Fact]
    public async Task GetIndustryFacilitiesAsync_ReturnsFacilities()
    {
        var facilities = new[]
        {
            new { facility_id = 1L, owner_id = 1000125, region_id = 10000002, solar_system_id = 30000142, type_id = 35825 }
        };
        _handler.QueueJsonResponse(facilities);

        var result = await _svc.GetIndustryFacilitiesAsync();

        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetIndustrySystemsAsync_ReturnsSystems()
    {
        var systems = new[]
        {
            new { solar_system_id = 30000142, cost_indices = new[] { new { activity = "manufacturing", cost_index = 0.05 } } }
        };
        _handler.QueueJsonResponse(systems);

        var result = await _svc.GetIndustrySystemsAsync();

        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetSovereigntyMapAsync_ReturnsSovMap()
    {
        var sovMap = new[]
        {
            new { system_id = 30000001, alliance_id = 99000001 }
        };
        _handler.QueueJsonResponse(sovMap);

        var result = await _svc.GetSovereigntyMapAsync();

        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetDogmaAttributesAsync_ReturnsAttributeIds()
    {
        _handler.QueueJsonResponse(new[] { 1L, 2L, 3L, 4L, 5L });

        var result = await _svc.GetDogmaAttributesAsync();

        Assert.NotNull(result);
        Assert.Equal(5, result.Count);
    }

    [Fact]
    public async Task GetDogmaAttributeAsync_ReturnsDetails()
    {
        var attr = new
        {
            attribute_id = 37,
            name = "maxVelocity",
            description = "Maximum velocity",
            default_value = 0.0,
            high_is_good = true
        };
        _handler.QueueJsonResponse(attr);

        var result = await _svc.GetDogmaAttributeAsync(37);

        Assert.NotNull(result);
        Assert.Equal("maxVelocity", result.Name);
    }

    [Fact]
    public async Task GetLoyaltyStoreOffersAsync_ReturnsOffers()
    {
        var offers = new[]
        {
            new { offer_id = 1, type_id = 34, quantity = 1000, lp_cost = 100, isk_cost = 0 }
        };
        _handler.QueueJsonResponse(offers);

        var result = await _svc.GetLoyaltyStoreOffersAsync(1000125);

        Assert.NotNull(result);
        Assert.Single(result);
    }
}
