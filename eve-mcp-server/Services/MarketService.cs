using eve_mcp_server.Infrastructure;
using eve_mcp_server.Models;

namespace eve_mcp_server.Services;

/// <summary>
/// Service for EVE Market endpoints (prices, orders, history, market groups).
/// All endpoints are public and do not require authentication.
/// </summary>
public sealed class MarketService
{
    private readonly EsiClient _client;

    public MarketService(EsiClient client) => _client = client;

    // ── Market Prices ────────────────────────────────────

    /// <summary>Get a list of average and adjusted prices for all item types.</summary>
    public Task<List<MarketPrice>?> GetPricesAsync(CancellationToken ct = default)
        => _client.GetAsync<List<MarketPrice>>("/markets/prices/", ct);

    // ── Market Orders ────────────────────────────────────

    /// <summary>
    /// Get market orders in a region, optionally filtered by type.
    /// </summary>
    public Task<List<MarketOrder>?> GetOrdersAsync(long regionId, string orderType = "all", long? typeId = null, int page = 1, CancellationToken ct = default)
    {
        var url = $"/markets/{regionId}/orders/?order_type={orderType}&page={page}";
        if (typeId.HasValue)
            url += $"&type_id={typeId.Value}";
        return _client.GetAsync<List<MarketOrder>>(url, ct);
    }

    // ── Market History ───────────────────────────────────

    /// <summary>Get historical market statistics for a type in a region.</summary>
    public Task<List<MarketHistory>?> GetHistoryAsync(long regionId, long typeId, CancellationToken ct = default)
        => _client.GetAsync<List<MarketHistory>>($"/markets/{regionId}/history/?type_id={typeId}", ct);

    // ── Market Groups ────────────────────────────────────

    /// <summary>Get a list of market group IDs.</summary>
    public Task<List<long>?> GetMarketGroupsAsync(CancellationToken ct = default)
        => _client.GetAsync<List<long>>("/markets/groups/", ct);

    /// <summary>Get details about a specific market group.</summary>
    public Task<MarketGroup?> GetMarketGroupAsync(long marketGroupId, CancellationToken ct = default)
        => _client.GetAsync<MarketGroup>($"/markets/groups/{marketGroupId}/", ct);

    // ── Types in Region ──────────────────────────────────

    /// <summary>Get a list of type IDs that have active orders in a region.</summary>
    public Task<List<long>?> GetTypesInRegionAsync(long regionId, int page = 1, CancellationToken ct = default)
        => _client.GetAsync<List<long>>($"/markets/{regionId}/types/?page={page}", ct);
}
