using System.ComponentModel;
using System.Text.Json;
using eve_mcp_server.Services;
using ModelContextProtocol.Server;

namespace eve_mcp_server.Tools;

[McpServerToolType]
public static class MarketTools
{
    [McpServerTool, Description("Get average and adjusted prices for all EVE item types. Useful for getting a quick overview of item values.")]
    public static async Task<string> GetMarketPrices(MarketService svc, CancellationToken ct)
    {
        var result = await svc.GetPricesAsync(ct);
        return JsonSerializer.Serialize(result);
    }

    [McpServerTool, Description("Get market orders in a specific region, optionally filtered by item type. Returns buy/sell orders with prices, volumes, and locations.")]
    public static async Task<string> GetMarketOrders(
        MarketService svc,
        [Description("The region ID to search (e.g., 10000002 for The Forge/Jita)")] long regionId,
        [Description("Filter by order type: 'buy', 'sell', or 'all' (default: 'all')")] string orderType = "all",
        [Description("Optional: filter by specific item type ID")] long? typeId = null,
        [Description("Page number (default: 1)")] int page = 1,
        CancellationToken ct = default)
    {
        var result = await svc.GetOrdersAsync(regionId, orderType, typeId, page, ct);
        return JsonSerializer.Serialize(result);
    }

    [McpServerTool, Description("Get daily market history (average price, volume, order count) for an item type in a region.")]
    public static async Task<string> GetMarketHistory(
        MarketService svc,
        [Description("The region ID (e.g., 10000002 for The Forge)")] long regionId,
        [Description("The item type ID")] long typeId,
        CancellationToken ct = default)
    {
        var result = await svc.GetHistoryAsync(regionId, typeId, ct);
        return JsonSerializer.Serialize(result);
    }

    [McpServerTool, Description("Get all market group IDs. Market groups organize items into a browsable hierarchy.")]
    public static async Task<string> GetMarketGroups(MarketService svc, CancellationToken ct)
    {
        var result = await svc.GetMarketGroupsAsync(ct);
        return JsonSerializer.Serialize(result);
    }

    [McpServerTool, Description("Get details about a specific market group including its name, description, parent group, and child type IDs.")]
    public static async Task<string> GetMarketGroup(
        MarketService svc,
        [Description("The market group ID")] long marketGroupId,
        CancellationToken ct = default)
    {
        var result = await svc.GetMarketGroupAsync(marketGroupId, ct);
        return result is null ? "Market group not found." : JsonSerializer.Serialize(result);
    }

    [McpServerTool, Description("Get a list of type IDs that have active market orders in a region.")]
    public static async Task<string> GetTypesInRegion(
        MarketService svc,
        [Description("The region ID")] long regionId,
        [Description("Page number (default: 1)")] int page = 1,
        CancellationToken ct = default)
    {
        var result = await svc.GetTypesInRegionAsync(regionId, page, ct);
        return JsonSerializer.Serialize(result);
    }
}
