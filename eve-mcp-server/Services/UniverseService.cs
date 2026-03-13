using eve_mcp_server.Infrastructure;
using eve_mcp_server.Models;

namespace eve_mcp_server.Services;

/// <summary>
/// Service for EVE Universe endpoints (regions, constellations, systems, types, etc.).
/// All endpoints are public and do not require authentication.
/// </summary>
public sealed class UniverseService
{
    private readonly EsiClient _client;

    public UniverseService(EsiClient client) => _client = client;

    // ── Regions ──────────────────────────────────────────

    /// <summary>Get all region IDs.</summary>
    public Task<List<long>?> GetRegionsAsync(CancellationToken ct = default)
        => _client.GetAsync<List<long>>("/universe/regions/", ct);

    /// <summary>Get details for a specific region.</summary>
    public Task<Region?> GetRegionAsync(long regionId, CancellationToken ct = default)
        => _client.GetAsync<Region>($"/universe/regions/{regionId}/", ct);

    // ── Constellations ───────────────────────────────────

    /// <summary>Get all constellation IDs.</summary>
    public Task<List<long>?> GetConstellationsAsync(CancellationToken ct = default)
        => _client.GetAsync<List<long>>("/universe/constellations/", ct);

    /// <summary>Get details for a specific constellation.</summary>
    public Task<Constellation?> GetConstellationAsync(long constellationId, CancellationToken ct = default)
        => _client.GetAsync<Constellation>($"/universe/constellations/{constellationId}/", ct);

    // ── Solar Systems ────────────────────────────────────

    /// <summary>Get all solar system IDs.</summary>
    public Task<List<long>?> GetSystemsAsync(CancellationToken ct = default)
        => _client.GetAsync<List<long>>("/universe/systems/", ct);

    /// <summary>Get details for a specific solar system.</summary>
    public Task<SolarSystem?> GetSystemAsync(long systemId, CancellationToken ct = default)
        => _client.GetAsync<SolarSystem>($"/universe/systems/{systemId}/", ct);

    // ── Stars ────────────────────────────────────────────

    /// <summary>Get details for a star.</summary>
    public Task<Star?> GetStarAsync(long starId, CancellationToken ct = default)
        => _client.GetAsync<Star>($"/universe/stars/{starId}/", ct);

    // ── Stations ─────────────────────────────────────────

    /// <summary>Get details for an NPC station.</summary>
    public Task<Station?> GetStationAsync(long stationId, CancellationToken ct = default)
        => _client.GetAsync<Station>($"/universe/stations/{stationId}/", ct);

    // ── Stargates ────────────────────────────────────────

    /// <summary>Get details for a stargate.</summary>
    public Task<Stargate?> GetStargateAsync(long stargateId, CancellationToken ct = default)
        => _client.GetAsync<Stargate>($"/universe/stargates/{stargateId}/", ct);

    // ── Planets & Moons ──────────────────────────────────

    /// <summary>Get details for a planet.</summary>
    public Task<Planet?> GetPlanetAsync(long planetId, CancellationToken ct = default)
        => _client.GetAsync<Planet>($"/universe/planets/{planetId}/", ct);

    /// <summary>Get details for a moon.</summary>
    public Task<Moon?> GetMoonAsync(long moonId, CancellationToken ct = default)
        => _client.GetAsync<Moon>($"/universe/moons/{moonId}/", ct);

    // ── Types ────────────────────────────────────────────

    /// <summary>Get details for an item type.</summary>
    public Task<EveType?> GetTypeAsync(long typeId, CancellationToken ct = default)
        => _client.GetAsync<EveType>($"/universe/types/{typeId}/", ct);

    // ── Groups ───────────────────────────────────────────

    /// <summary>Get details for an item group.</summary>
    public Task<ItemGroup?> GetGroupAsync(long groupId, CancellationToken ct = default)
        => _client.GetAsync<ItemGroup>($"/universe/groups/{groupId}/", ct);

    // ── Categories ───────────────────────────────────────

    /// <summary>Get all item category IDs.</summary>
    public Task<List<long>?> GetCategoriesAsync(CancellationToken ct = default)
        => _client.GetAsync<List<long>>("/universe/categories/", ct);

    /// <summary>Get details for an item category.</summary>
    public Task<ItemCategory?> GetCategoryAsync(long categoryId, CancellationToken ct = default)
        => _client.GetAsync<ItemCategory>($"/universe/categories/{categoryId}/", ct);

    // ── Factions, Races, Bloodlines, Ancestries ──────────

    /// <summary>Get all factions.</summary>
    public Task<List<Faction>?> GetFactionsAsync(CancellationToken ct = default)
        => _client.GetAsync<List<Faction>>("/universe/factions/", ct);

    /// <summary>Get all playable races.</summary>
    public Task<List<Race>?> GetRacesAsync(CancellationToken ct = default)
        => _client.GetAsync<List<Race>>("/universe/races/", ct);

    /// <summary>Get all playable bloodlines.</summary>
    public Task<List<Bloodline>?> GetBloodlinesAsync(CancellationToken ct = default)
        => _client.GetAsync<List<Bloodline>>("/universe/bloodlines/", ct);

    /// <summary>Get all playable ancestries.</summary>
    public Task<List<Ancestry>?> GetAncestriesAsync(CancellationToken ct = default)
        => _client.GetAsync<List<Ancestry>>("/universe/ancestries/", ct);

    // ── System Activity ──────────────────────────────────

    /// <summary>Get system jump counts for the last hour.</summary>
    public Task<List<SystemJumps>?> GetSystemJumpsAsync(CancellationToken ct = default)
        => _client.GetAsync<List<SystemJumps>>("/universe/system_jumps/", ct);

    /// <summary>Get system kill counts for the last hour.</summary>
    public Task<List<SystemKills>?> GetSystemKillsAsync(CancellationToken ct = default)
        => _client.GetAsync<List<SystemKills>>("/universe/system_kills/", ct);
}
