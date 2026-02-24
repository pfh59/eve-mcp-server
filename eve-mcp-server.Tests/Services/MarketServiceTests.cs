using eve_mcp_server.Models;
using eve_mcp_server.Services;

namespace eve_mcp_server.Tests.Services;

public class MarketServiceTests
{
    private readonly MarketService _svc;
    private readonly MockHttpMessageHandler _handler;

    public MarketServiceTests()
    {
        var (client, handler) = TestEsiClientFactory.Create();
        _handler = handler;
        _svc = new MarketService(client);
    }

    [Fact]
    public async Task GetPricesAsync_ReturnsPrices()
    {
        var prices = new[]
        {
            new { type_id = 34, average_price = 5.5, adjusted_price = 5.2 },
            new { type_id = 35, average_price = 10.0, adjusted_price = 9.8 }
        };
        _handler.QueueJsonResponse(prices);

        var result = await _svc.GetPricesAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetOrdersAsync_IncludesRegionAndOrderType()
    {
        _handler.QueueJsonResponse(Array.Empty<object>());

        await _svc.GetOrdersAsync(10000002, "sell", typeId: 34, page: 1);

        var uri = _handler.LastRequest.RequestUri!.ToString();
        Assert.Contains("/markets/10000002/orders/", uri);
        Assert.Contains("order_type=sell", uri);
        Assert.Contains("type_id=34", uri);
    }

    [Fact]
    public async Task GetOrdersAsync_WithoutTypeId_OmitsTypeIdParam()
    {
        _handler.QueueJsonResponse(Array.Empty<object>());

        await _svc.GetOrdersAsync(10000002);

        var uri = _handler.LastRequest.RequestUri!.ToString();
        Assert.DoesNotContain("type_id", uri);
    }

    [Fact]
    public async Task GetHistoryAsync_ReturnsHistory()
    {
        var history = new[]
        {
            new { date = "2024-01-01", average = 5.5, highest = 6.0, lowest = 5.0, order_count = 100, volume = 1000000L },
            new { date = "2024-01-02", average = 5.6, highest = 6.1, lowest = 5.1, order_count = 120, volume = 1200000L }
        };
        _handler.QueueJsonResponse(history);

        var result = await _svc.GetHistoryAsync(10000002, 34);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetMarketGroupsAsync_ReturnsIds()
    {
        _handler.QueueJsonResponse(new[] { 1L, 2L, 3L });

        var result = await _svc.GetMarketGroupsAsync();

        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetMarketGroupAsync_ReturnsGroupDetails()
    {
        var group = new { market_group_id = 4, name = "Ships", description = "All ships" };
        _handler.QueueJsonResponse(group);

        var result = await _svc.GetMarketGroupAsync(4);

        Assert.NotNull(result);
        Assert.Equal("Ships", result.Name);
    }

    [Fact]
    public async Task GetTypesInRegionAsync_ReturnsTypeIds()
    {
        _handler.QueueJsonResponse(new[] { 34L, 35L, 36L });

        var result = await _svc.GetTypesInRegionAsync(10000002);

        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
    }
}
