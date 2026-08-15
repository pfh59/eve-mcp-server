using System.ComponentModel;
using eve_mcp_server.Services;
using ModelContextProtocol.Server;

namespace eve_mcp_server.Tools;

/// <summary>
/// MCP tools for the EVE Universe: regions, systems, types, factions, activity, etc.
/// </summary>
[McpServerToolType]
public static class UniverseTools
{
    // ── Regions ──────────────────────────────────────────

    /// <summary>List all region IDs.</summary>
    [McpServerTool(Name = "eve_get_regions"), Description("Get a list of all EVE Online region IDs.")]
    public static Task<string> GetRegions(UniverseService svc, CancellationToken ct)
        => ToolRunner.RunAsync(() => svc.GetRegionsAsync(ct));

    /// <summary>Get details for a specific region.</summary>
    [McpServerTool(Name = "eve_get_region"), Description("Get details about a specific EVE Online region including its name, description, and constellation IDs.")]
    public static Task<string> GetRegion(
        UniverseService svc,
        [Description("The EVE region ID")] long regionId,
        CancellationToken ct)
        => ToolRunner.RunAsync(() => svc.GetRegionAsync(regionId, ct), "Region not found.");

    // ── Constellations ─────────────────────────────────────────────────

    [McpServerTool(Name = "eve_get_constellations"), Description("Get a list of all EVE Online constellation IDs.")]
    public static Task<string> GetConstellations(UniverseService svc, CancellationToken ct)
        => ToolRunner.RunAsync(() => svc.GetConstellationsAsync(ct));

    /// <summary>Get details for a specific constellation.</summary>
    [McpServerTool(Name = "eve_get_constellation"), Description("Get details about a specific constellation including its name, region, and system IDs.")]
    public static Task<string> GetConstellation(
        UniverseService svc,
        [Description("The EVE constellation ID")] long constellationId,
        CancellationToken ct)
        => ToolRunner.RunAsync(() => svc.GetConstellationAsync(constellationId, ct), "Constellation not found.");

    // ── Solar Systems ──────────────────────────────────────────────────

    [McpServerTool(Name = "eve_get_solar_systems"), Description("Get a list of all EVE Online solar system IDs.")]
    public static Task<string> GetSolarSystems(UniverseService svc, CancellationToken ct)
        => ToolRunner.RunAsync(() => svc.GetSystemsAsync(ct));

    /// <summary>Get details for a specific solar system.</summary>
    [McpServerTool(Name = "eve_get_solar_system"), Description("Get details about a specific solar system including its name, security status, constellation, stations, stargates, and planets.")]
    public static Task<string> GetSolarSystem(
        UniverseService svc,
        [Description("The EVE solar system ID")] long systemId,
        CancellationToken ct)
        => ToolRunner.RunAsync(() => svc.GetSystemAsync(systemId, ct), "Solar system not found.");

    // ── Stars, Stations, Stargates ─────────────────────────────────────

    [McpServerTool(Name = "eve_get_star"), Description("Get details about a star (type, name, luminosity, temperature, etc.).")]
    public static Task<string> GetStar(
        UniverseService svc,
        [Description("The EVE star ID")] long starId,
        CancellationToken ct)
        => ToolRunner.RunAsync(() => svc.GetStarAsync(starId, ct), "Star not found.");

    /// <summary>Get details for an NPC station.</summary>
    [McpServerTool(Name = "eve_get_station"), Description("Get details about an NPC station (name, owner, type, services, etc.).")]
    public static Task<string> GetStation(
        UniverseService svc,
        [Description("The EVE station ID")] long stationId,
        CancellationToken ct)
        => ToolRunner.RunAsync(() => svc.GetStationAsync(stationId, ct), "Station not found.");

    /// <summary>Get details for a stargate.</summary>
    [McpServerTool(Name = "eve_get_stargate"), Description("Get details about a stargate (name, destination system/stargate).")]
    public static Task<string> GetStargate(
        UniverseService svc,
        [Description("The EVE stargate ID")] long stargateId,
        CancellationToken ct)
        => ToolRunner.RunAsync(() => svc.GetStargateAsync(stargateId, ct), "Stargate not found.");

    // ── Planets & Moons ────────────────────────────────────────────────

    /// <summary>Get details for a planet.</summary>
    [McpServerTool(Name = "eve_get_planet"), Description("Get details about a planet (name, type, position).")]
    public static Task<string> GetPlanet(
        UniverseService svc,
        [Description("The EVE planet ID")] long planetId,
        CancellationToken ct)
        => ToolRunner.RunAsync(() => svc.GetPlanetAsync(planetId, ct), "Planet not found.");

    /// <summary>Get details for a moon.</summary>
    [McpServerTool(Name = "eve_get_moon"), Description("Get details about a moon (name, position).")]
    public static Task<string> GetMoon(
        UniverseService svc,
        [Description("The EVE moon ID")] long moonId,
        CancellationToken ct)
        => ToolRunner.RunAsync(() => svc.GetMoonAsync(moonId, ct), "Moon not found.");

    // ── Types & Groups & Categories ────────────────────────────────────

    /// <summary>Get detailed info about an item type.</summary>
    [McpServerTool(Name = "eve_get_type"), Description("Get detailed info about an EVE item type (name, description, mass, volume, capacity, dogma attributes/effects, etc.).")]
    public static Task<string> GetType(
        UniverseService svc,
        [Description("The EVE type ID")] long typeId,
        CancellationToken ct)
        => ToolRunner.RunAsync(() => svc.GetTypeAsync(typeId, ct), "Type not found.");

    /// <summary>Get details for an item group.</summary>
    [McpServerTool(Name = "eve_get_item_group"), Description("Get details about an item group (name, category, type IDs).")]
    public static Task<string> GetItemGroup(
        UniverseService svc,
        [Description("The item group ID")] long groupId,
        CancellationToken ct)
        => ToolRunner.RunAsync(() => svc.GetGroupAsync(groupId, ct), "Group not found.");

    /// <summary>List all item category IDs.</summary>
    [McpServerTool(Name = "eve_get_categories"), Description("Get a list of all item category IDs.")]
    public static Task<string> GetCategories(UniverseService svc, CancellationToken ct)
        => ToolRunner.RunAsync(() => svc.GetCategoriesAsync(ct));

    /// <summary>Get details for an item category.</summary>
    [McpServerTool(Name = "eve_get_category"), Description("Get details about an item category (name, group IDs).")]
    public static Task<string> GetCategory(
        UniverseService svc,
        [Description("The item category ID")] long categoryId,
        CancellationToken ct)
        => ToolRunner.RunAsync(() => svc.GetCategoryAsync(categoryId, ct), "Category not found.");

    // ── Factions, Races, Bloodlines, Ancestries ────────────────────────

    /// <summary>Get all factions.</summary>
    [McpServerTool(Name = "eve_get_factions"), Description("Get all EVE Online factions with their details (name, description, station systems, corporation, militia).")]
    public static Task<string> GetFactions(UniverseService svc, CancellationToken ct)
        => ToolRunner.RunAsync(() => svc.GetFactionsAsync(ct));

    /// <summary>Get all playable races.</summary>
    [McpServerTool(Name = "eve_get_races"), Description("Get all EVE Online playable races with their details.")]
    public static Task<string> GetRaces(UniverseService svc, CancellationToken ct)
        => ToolRunner.RunAsync(() => svc.GetRacesAsync(ct));

    /// <summary>Get all playable bloodlines.</summary>
    [McpServerTool(Name = "eve_get_bloodlines"), Description("Get all EVE Online playable bloodlines with their details.")]
    public static Task<string> GetBloodlines(UniverseService svc, CancellationToken ct)
        => ToolRunner.RunAsync(() => svc.GetBloodlinesAsync(ct));

    /// <summary>Get all playable ancestries.</summary>
    [McpServerTool(Name = "eve_get_ancestries"), Description("Get all EVE Online playable ancestries with their details.")]
    public static Task<string> GetAncestries(UniverseService svc, CancellationToken ct)
        => ToolRunner.RunAsync(() => svc.GetAncestriesAsync(ct));

    // ── System Activity ────────────────────────────────────────────────

    /// <summary>Get ship jumps per system in the last hour.</summary>
    [McpServerTool(Name = "eve_get_system_jumps"), Description("Get the number of ship jumps per solar system in the last hour.")]
    public static Task<string> GetSystemJumps(UniverseService svc, CancellationToken ct)
        => ToolRunner.RunAsync(() => svc.GetSystemJumpsAsync(ct));

    /// <summary>Get NPC and player kills per system in the last hour.</summary>
    [McpServerTool(Name = "eve_get_system_kills"), Description("Get the number of NPC and player ship kills per solar system in the last hour.")]
    public static Task<string> GetSystemKills(UniverseService svc, CancellationToken ct)
        => ToolRunner.RunAsync(() => svc.GetSystemKillsAsync(ct));
}
