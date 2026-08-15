using System.ComponentModel;
using eve_mcp_server.Services;
using ModelContextProtocol.Server;

namespace eve_mcp_server.Tools;

/// <summary>
/// MCP tools for EVE Online market data (prices, orders, history, groups).
/// </summary>
[McpServerToolType]
public static class MarketTools
{
    /// <summary>Get average/adjusted prices for all tradeable types.</summary>
    [McpServerTool(Name = "eve_get_market_prices"), Description("Get average and adjusted prices for all EVE item types. Useful for getting a quick overview of item values.")]
    public static Task<string> GetMarketPrices(MarketService svc, CancellationToken ct)
        => ToolRunner.RunAsync(() => svc.GetPricesAsync(ct));

    /// <summary>Get market orders in a region, optionally filtered by type.</summary>
    [McpServerTool(Name = "eve_get_market_orders"), Description("Get market orders in a specific region, optionally filtered by item type. Returns buy/sell orders with prices, volumes, and locations.")]
    public static Task<string> GetMarketOrders(
        MarketService svc,
        [Description("The region ID to search (e.g., 10000002 for The Forge/Jita)")] long regionId,
        [Description("Filter by order type: 'buy', 'sell', or 'all' (default: 'all')")] string orderType = "all",
        [Description("Optional: filter by specific item type ID")] long? typeId = null,
        [Description("Page number (default: 1)")] int page = 1,
        CancellationToken ct = default)
        => ToolRunner.RunAsync(() => svc.GetOrdersAsync(regionId, orderType, typeId, page, ct));

    /// <summary>Get daily price/volume history for a type in a region.</summary>
    [McpServerTool(Name = "eve_get_market_history"), Description("Get daily market history (average price, volume, order count) for an item type in a region.")]
    public static Task<string> GetMarketHistory(
        MarketService svc,
        [Description("The region ID (e.g., 10000002 for The Forge)")] long regionId,
        [Description("The item type ID")] long typeId,
        CancellationToken ct = default)
        => ToolRunner.RunAsync(() => svc.GetHistoryAsync(regionId, typeId, ct));

    /// <summary>List all market group IDs.</summary>
    [McpServerTool(Name = "eve_get_market_groups"), Description("Get all market group IDs. Market groups organize items into a browsable hierarchy.")]
    public static Task<string> GetMarketGroups(MarketService svc, CancellationToken ct)
        => ToolRunner.RunAsync(() => svc.GetMarketGroupsAsync(ct));

    /// <summary>Get a single market group with name, parent, and types.</summary>
    [McpServerTool(Name = "eve_get_market_group"), Description("Get details about a specific market group including its name, description, parent group, and child type IDs.")]
    public static Task<string> GetMarketGroup(
        MarketService svc,
        [Description("The market group ID")] long marketGroupId,
        CancellationToken ct = default)
        => ToolRunner.RunAsync(() => svc.GetMarketGroupAsync(marketGroupId, ct), "Market group not found.");

    /// <summary>List type IDs with active orders in a region.</summary>
    [McpServerTool(Name = "eve_get_types_in_region"), Description("Get a list of type IDs that have active market orders in a region.")]
    public static Task<string> GetTypesInRegion(
        MarketService svc,
        [Description("The region ID")] long regionId,
        [Description("Page number (default: 1)")] int page = 1,
        CancellationToken ct = default)
        => ToolRunner.RunAsync(() => svc.GetTypesInRegionAsync(regionId, page, ct));
}
