using System.ComponentModel;
using System.Text.Json;
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
    [McpServerTool, Description("Get a list of all EVE Online region IDs.")]
    public static async Task<string> GetRegions(UniverseService svc, CancellationToken ct)
    {
        var result = await svc.GetRegionsAsync(ct);
        return JsonSerializer.Serialize(result);
    }

    /// <summary>Get details for a specific region.</summary>
    [McpServerTool, Description("Get details about a specific EVE Online region including its name, description, and constellation IDs.")]
    public static async Task<string> GetRegion(
        UniverseService svc,
        [Description("The EVE region ID")] long regionId,
        CancellationToken ct)
    {
        var result = await svc.GetRegionAsync(regionId, ct);
        return result is null ? "Region not found." : JsonSerializer.Serialize(result);
    }

    // ── Constellations ─────────────────────────────────────────────────

    [McpServerTool, Description("Get a list of all EVE Online constellation IDs.")]
    public static async Task<string> GetConstellations(UniverseService svc, CancellationToken ct)
    {
        var result = await svc.GetConstellationsAsync(ct);
        return JsonSerializer.Serialize(result);
    }

    /// <summary>Get details for a specific constellation.</summary>
    [McpServerTool, Description("Get details about a specific constellation including its name, region, and system IDs.")]
    public static async Task<string> GetConstellation(
        UniverseService svc,
        [Description("The EVE constellation ID")] long constellationId,
        CancellationToken ct)
    {
        var result = await svc.GetConstellationAsync(constellationId, ct);
        return result is null ? "Constellation not found." : JsonSerializer.Serialize(result);
    }

    // ── Solar Systems ──────────────────────────────────────────────────

    [McpServerTool, Description("Get a list of all EVE Online solar system IDs.")]
    public static async Task<string> GetSolarSystems(UniverseService svc, CancellationToken ct)
    {
        var result = await svc.GetSystemsAsync(ct);
        return JsonSerializer.Serialize(result);
    }

    /// <summary>Get details for a specific solar system.</summary>
    [McpServerTool, Description("Get details about a specific solar system including its name, security status, constellation, stations, stargates, and planets.")]
    public static async Task<string> GetSolarSystem(
        UniverseService svc,
        [Description("The EVE solar system ID")] long systemId,
        CancellationToken ct)
    {
        var result = await svc.GetSystemAsync(systemId, ct);
        return result is null ? "Solar system not found." : JsonSerializer.Serialize(result);
    }

    // ── Stars, Stations, Stargates ─────────────────────────────────────

    [McpServerTool, Description("Get details about a star (type, name, luminosity, temperature, etc.).")]
    public static async Task<string> GetStar(
        UniverseService svc,
        [Description("The EVE star ID")] long starId,
        CancellationToken ct)
    {
        var result = await svc.GetStarAsync(starId, ct);
        return result is null ? "Star not found." : JsonSerializer.Serialize(result);
    }

    /// <summary>Get details for an NPC station.</summary>
    [McpServerTool, Description("Get details about an NPC station (name, owner, type, services, etc.).")]
    public static async Task<string> GetStation(
        UniverseService svc,
        [Description("The EVE station ID")] long stationId,
        CancellationToken ct)
    {
        var result = await svc.GetStationAsync(stationId, ct);
        return result is null ? "Station not found." : JsonSerializer.Serialize(result);
    }

    /// <summary>Get details for a stargate.</summary>
    [McpServerTool, Description("Get details about a stargate (name, destination system/stargate).")]
    public static async Task<string> GetStargate(
        UniverseService svc,
        [Description("The EVE stargate ID")] long stargateId,
        CancellationToken ct)
    {
        var result = await svc.GetStargateAsync(stargateId, ct);
        return result is null ? "Stargate not found." : JsonSerializer.Serialize(result);
    }

    // ── Planets & Moons ────────────────────────────────────────────────

    /// <summary>Get details for a planet.</summary>
    [McpServerTool, Description("Get details about a planet (name, type, position).")]
    public static async Task<string> GetPlanet(
        UniverseService svc,
        [Description("The EVE planet ID")] long planetId,
        CancellationToken ct)
    {
        var result = await svc.GetPlanetAsync(planetId, ct);
        return result is null ? "Planet not found." : JsonSerializer.Serialize(result);
    }

    /// <summary>Get details for a moon.</summary>
    [McpServerTool, Description("Get details about a moon (name, position).")]
    public static async Task<string> GetMoon(
        UniverseService svc,
        [Description("The EVE moon ID")] long moonId,
        CancellationToken ct)
    {
        var result = await svc.GetMoonAsync(moonId, ct);
        return result is null ? "Moon not found." : JsonSerializer.Serialize(result);
    }

    // ── Types & Groups & Categories ────────────────────────────────────

    /// <summary>Get detailed info about an item type.</summary>
    [McpServerTool, Description("Get detailed info about an EVE item type (name, description, mass, volume, capacity, dogma attributes/effects, etc.).")]
    public static async Task<string> GetType(
        UniverseService svc,
        [Description("The EVE type ID")] long typeId,
        CancellationToken ct)
    {
        var result = await svc.GetTypeAsync(typeId, ct);
        return result is null ? "Type not found." : JsonSerializer.Serialize(result);
    }

    /// <summary>Get details for an item group.</summary>
    [McpServerTool, Description("Get details about an item group (name, category, type IDs).")]
    public static async Task<string> GetItemGroup(
        UniverseService svc,
        [Description("The item group ID")] long groupId,
        CancellationToken ct)
    {
        var result = await svc.GetGroupAsync(groupId, ct);
        return result is null ? "Group not found." : JsonSerializer.Serialize(result);
    }

    /// <summary>List all item category IDs.</summary>
    [McpServerTool, Description("Get a list of all item category IDs.")]
    public static async Task<string> GetCategories(UniverseService svc, CancellationToken ct)
    {
        var result = await svc.GetCategoriesAsync(ct);
        return JsonSerializer.Serialize(result);
    }

    /// <summary>Get details for an item category.</summary>
    [McpServerTool, Description("Get details about an item category (name, group IDs).")]
    public static async Task<string> GetCategory(
        UniverseService svc,
        [Description("The item category ID")] long categoryId,
        CancellationToken ct)
    {
        var result = await svc.GetCategoryAsync(categoryId, ct);
        return result is null ? "Category not found." : JsonSerializer.Serialize(result);
    }

    // ── Factions, Races, Bloodlines, Ancestries ────────────────────────

    /// <summary>Get all factions.</summary>
    [McpServerTool, Description("Get all EVE Online factions with their details (name, description, station systems, corporation, militia).")]
    public static async Task<string> GetFactions(UniverseService svc, CancellationToken ct)
    {
        var result = await svc.GetFactionsAsync(ct);
        return JsonSerializer.Serialize(result);
    }

    /// <summary>Get all playable races.</summary>
    [McpServerTool, Description("Get all EVE Online playable races with their details.")]
    public static async Task<string> GetRaces(UniverseService svc, CancellationToken ct)
    {
        var result = await svc.GetRacesAsync(ct);
        return JsonSerializer.Serialize(result);
    }

    /// <summary>Get all playable bloodlines.</summary>
    [McpServerTool, Description("Get all EVE Online playable bloodlines with their details.")]
    public static async Task<string> GetBloodlines(UniverseService svc, CancellationToken ct)
    {
        var result = await svc.GetBloodlinesAsync(ct);
        return JsonSerializer.Serialize(result);
    }

    /// <summary>Get all playable ancestries.</summary>
    [McpServerTool, Description("Get all EVE Online playable ancestries with their details.")]
    public static async Task<string> GetAncestries(UniverseService svc, CancellationToken ct)
    {
        var result = await svc.GetAncestriesAsync(ct);
        return JsonSerializer.Serialize(result);
    }

    // ── System Activity ────────────────────────────────────────────────

    /// <summary>Get ship jumps per system in the last hour.</summary>
    [McpServerTool, Description("Get the number of ship jumps per solar system in the last hour.")]
    public static async Task<string> GetSystemJumps(UniverseService svc, CancellationToken ct)
    {
        var result = await svc.GetSystemJumpsAsync(ct);
        return JsonSerializer.Serialize(result);
    }

    /// <summary>Get NPC and player kills per system in the last hour.</summary>
    [McpServerTool, Description("Get the number of NPC and player ship kills per solar system in the last hour.")]
    public static async Task<string> GetSystemKills(UniverseService svc, CancellationToken ct)
    {
        var result = await svc.GetSystemKillsAsync(ct);
        return JsonSerializer.Serialize(result);
    }
}
