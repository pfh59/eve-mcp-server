using eve_mcp_server.Services;
using eve_mcp_server.Tools;

namespace eve_mcp_server.Tests.Tools;

public class MarketToolsTests
{
    private readonly MarketService _svc;
    private readonly MockHttpMessageHandler _handler;

    public MarketToolsTests()
    {
        var (client, handler) = TestEsiClientFactory.Create();
        _handler = handler;
        _svc = new MarketService(client);
    }

    [Fact]
    public async Task GetMarketPrices_ReturnsJson()
    {
        _handler.QueueJsonResponse(new[]
        {
            new { type_id = 34, average_price = 5.5, adjusted_price = 5.2 }
        });

        var result = await MarketTools.GetMarketPrices(_svc, CancellationToken.None);

        Assert.Contains("34", result);
        Assert.Contains("5.5", result);
    }

    [Fact]
    public async Task GetMarketOrders_PassesParametersCorrectly()
    {
        _handler.QueueJsonResponse(Array.Empty<object>());

        var result = await MarketTools.GetMarketOrders(_svc, 10000002, "sell", 34, 1, CancellationToken.None);

        var uri = _handler.LastRequest.RequestUri!.ToString();
        Assert.Contains("/markets/10000002/orders/", uri);
        Assert.Contains("order_type=sell", uri);
        Assert.Contains("type_id=34", uri);
    }

    [Fact]
    public async Task GetMarketHistory_ReturnsJson()
    {
        _handler.QueueJsonResponse(new[]
        {
            new { date = "2024-01-01", average = 5.5, volume = 1000000L }
        });

        var result = await MarketTools.GetMarketHistory(_svc, 10000002, 34, CancellationToken.None);

        Assert.Contains("2024-01-01", result);
    }

    [Fact]
    public async Task GetMarketGroup_WhenNotFound_ReturnsMessage()
    {
        _handler.Queue500();

        var result = await MarketTools.GetMarketGroup(_svc, 999999, CancellationToken.None);

        Assert.Equal("Market group not found.", result);
    }
}
