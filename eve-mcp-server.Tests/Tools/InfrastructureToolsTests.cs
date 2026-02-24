using eve_mcp_server.Services;
using eve_mcp_server.Tools;

namespace eve_mcp_server.Tests.Tools;

public class InfrastructureToolsTests
{
    private readonly InfrastructureService _svc;
    private readonly MockHttpMessageHandler _handler;

    public InfrastructureToolsTests()
    {
        var (client, handler) = TestEsiClientFactory.Create();
        _handler = handler;
        _svc = new InfrastructureService(client);
    }

    [Fact]
    public async Task GetRoute_ReturnsSystemList()
    {
        _handler.QueueJsonResponse(new[] { 30000142L, 30000144L, 30000148L });

        var result = await InfrastructureTools.GetRoute(_svc, 30000142, 30000148, CancellationToken.None);

        Assert.Contains("30000142", result);
        Assert.Contains("30000148", result);
    }

    [Fact]
    public async Task GetRoute_WhenFails_ReturnsMessage()
    {
        _handler.Queue500();

        var result = await InfrastructureTools.GetRoute(_svc, 1, 2, CancellationToken.None);

        Assert.Equal("Could not calculate route.", result);
    }

    [Fact]
    public async Task GetSovereigntyMap_ReturnsJson()
    {
        _handler.QueueJsonResponse(new[]
        {
            new { system_id = 30000001, alliance_id = 99000001 }
        });

        var result = await InfrastructureTools.GetSovereigntyMap(_svc, CancellationToken.None);

        Assert.Contains("30000001", result);
    }

    [Fact]
    public async Task GetDogmaAttribute_ReturnsDetails()
    {
        _handler.QueueJsonResponse(new
        {
            attribute_id = 37,
            name = "maxVelocity",
            description = "Maximum velocity"
        });

        var result = await InfrastructureTools.GetDogmaAttribute(_svc, 37, CancellationToken.None);

        Assert.Contains("maxVelocity", result);
    }

    [Fact]
    public async Task GetDogmaAttribute_WhenNotFound_ReturnsMessage()
    {
        _handler.Queue500();

        var result = await InfrastructureTools.GetDogmaAttribute(_svc, 999999, CancellationToken.None);

        Assert.Equal("Dogma attribute not found.", result);
    }

    [Fact]
    public async Task GetLoyaltyStoreOffers_ReturnsOffers()
    {
        _handler.QueueJsonResponse(new[]
        {
            new { offer_id = 1, type_id = 34, quantity = 1000, lp_cost = 100 }
        });

        var result = await InfrastructureTools.GetLoyaltyStoreOffers(_svc, 1000125, CancellationToken.None);

        Assert.Contains("34", result);
    }
}
