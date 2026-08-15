using System.ComponentModel;
using eve_mcp_server.Services;
using ModelContextProtocol.Server;

namespace eve_mcp_server.Tools;

/// <summary>
/// MCP tools for server status, killmails, wars, incursions, and insurance.
/// </summary>
[McpServerToolType]
public static class GameplayTools
{
    [McpServerTool(Name = "eve_get_server_status"), Description("Get EVE Online server status including player count, server version, and start time.")]
    public static Task<string> GetServerStatus(GameplayService svc, CancellationToken ct)
        => ToolRunner.RunAsync(() => svc.GetStatusAsync(ct), "Could not retrieve server status.");

    [McpServerTool(Name = "eve_get_killmail"), Description("Get details of a specific killmail by its ID and hash. Returns victim info, attackers, dropped/destroyed items, and solar system.")]
    public static Task<string> GetKillmail(
        GameplayService svc,
        [Description("The killmail ID")] long killmailId,
        [Description("The killmail hash (40-character hex, provided by zkillboard or other APIs)")] string killmailHash,
        CancellationToken ct = default)
        => ToolRunner.RunAsync(() => svc.GetKillmailAsync(killmailId, killmailHash, ct), "Killmail not found.");

    [McpServerTool(Name = "eve_get_wars"), Description("Get a list of recent war IDs (up to 2000, most recent first). Optionally provide a max_war_id to paginate.")]
    public static Task<string> GetWars(
        GameplayService svc,
        [Description("Optional: return only wars with ID less than this value for pagination")] long? maxWarId = null,
        CancellationToken ct = default)
        => ToolRunner.RunAsync(() => svc.GetWarsAsync(maxWarId, ct));

    [McpServerTool(Name = "eve_get_war"), Description("Get details about a specific war including aggressor, defender, allies, mutual status, and dates.")]
    public static Task<string> GetWar(
        GameplayService svc,
        [Description("The war ID")] long warId,
        CancellationToken ct = default)
        => ToolRunner.RunAsync(() => svc.GetWarAsync(warId, ct), "War not found.");

    [McpServerTool(Name = "eve_get_war_killmails"), Description("Get killmails associated with a war.")]
    public static Task<string> GetWarKillmails(
        GameplayService svc,
        [Description("The war ID")] long warId,
        [Description("Page number (default: 1)")] int page = 1,
        CancellationToken ct = default)
        => ToolRunner.RunAsync(() => svc.GetWarKillmailsAsync(warId, page, ct));

    [McpServerTool(Name = "eve_get_incursions"), Description("Get all current incursions including constellation, staging system, state, and influence.")]
    public static Task<string> GetIncursions(GameplayService svc, CancellationToken ct)
        => ToolRunner.RunAsync(() => svc.GetIncursionsAsync(ct));

    [McpServerTool(Name = "eve_get_insurance_prices"), Description("Get insurance prices for all ship types including payout levels.")]
    public static Task<string> GetInsurancePrices(GameplayService svc, CancellationToken ct)
        => ToolRunner.RunAsync(() => svc.GetInsurancePricesAsync(ct));
}
