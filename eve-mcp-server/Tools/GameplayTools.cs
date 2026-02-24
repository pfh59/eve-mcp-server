using System.ComponentModel;
using System.Text.Json;
using eve_mcp_server.Services;
using ModelContextProtocol.Server;

namespace eve_mcp_server.Tools;

[McpServerToolType]
public static class GameplayTools
{
    // ── Server Status ────────────────────────────────────

    [McpServerTool, Description("Get EVE Online server status including player count, server version, and start time.")]
    public static async Task<string> GetServerStatus(GameplayService svc, CancellationToken ct)
    {
        var result = await svc.GetStatusAsync(ct);
        return result is null ? "Could not retrieve server status." : JsonSerializer.Serialize(result);
    }

    // ── Killmails ────────────────────────────────────────

    [McpServerTool, Description("Get details of a specific killmail by its ID and hash. Returns victim info, attackers, dropped/destroyed items, and solar system.")]
    public static async Task<string> GetKillmail(
        GameplayService svc,
        [Description("The killmail ID")] long killmailId,
        [Description("The killmail hash (provided by zkillboard or other APIs)")] string killmailHash,
        CancellationToken ct = default)
    {
        var result = await svc.GetKillmailAsync(killmailId, killmailHash, ct);
        return result is null ? "Killmail not found." : JsonSerializer.Serialize(result);
    }

    // ── Wars ─────────────────────────────────────────────

    [McpServerTool, Description("Get a list of recent war IDs (up to 2000, most recent first). Optionally provide a max_war_id to paginate.")]
    public static async Task<string> GetWars(
        GameplayService svc,
        [Description("Optional: return only wars with ID less than this value for pagination")] long? maxWarId = null,
        CancellationToken ct = default)
    {
        var result = await svc.GetWarsAsync(maxWarId, ct);
        return JsonSerializer.Serialize(result);
    }

    [McpServerTool, Description("Get details about a specific war including aggressor, defender, allies, mutual status, and dates.")]
    public static async Task<string> GetWar(
        GameplayService svc,
        [Description("The war ID")] long warId,
        CancellationToken ct = default)
    {
        var result = await svc.GetWarAsync(warId, ct);
        return result is null ? "War not found." : JsonSerializer.Serialize(result);
    }

    [McpServerTool, Description("Get killmails associated with a war.")]
    public static async Task<string> GetWarKillmails(
        GameplayService svc,
        [Description("The war ID")] long warId,
        [Description("Page number (default: 1)")] int page = 1,
        CancellationToken ct = default)
    {
        var result = await svc.GetWarKillmailsAsync(warId, page, ct);
        return JsonSerializer.Serialize(result);
    }

    // ── Incursions ───────────────────────────────────────

    [McpServerTool, Description("Get all current incursions including constellation, staging system, state, and influence.")]
    public static async Task<string> GetIncursions(GameplayService svc, CancellationToken ct)
    {
        var result = await svc.GetIncursionsAsync(ct);
        return JsonSerializer.Serialize(result);
    }

    // ── Insurance ────────────────────────────────────────

    [McpServerTool, Description("Get insurance prices for all ship types including payout levels.")]
    public static async Task<string> GetInsurancePrices(GameplayService svc, CancellationToken ct)
    {
        var result = await svc.GetInsurancePricesAsync(ct);
        return JsonSerializer.Serialize(result);
    }
}
