using eve_mcp_server.Esi;
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

    public Task<List<long>?> GetRegionsAsync(CancellationToken ct = default)
        => _client.GetAsync<List<long>>("/universe/regions/", ct);

    public Task<Region?> GetRegionAsync(long regionId, CancellationToken ct = default)
        => _client.GetAsync<Region>($"/universe/regions/{regionId}/", ct);

    public Task<List<long>?> GetConstellationsAsync(CancellationToken ct = default)
        => _client.GetAsync<List<long>>("/universe/constellations/", ct);

    public Task<Constellation?> GetConstellationAsync(long constellationId, CancellationToken ct = default)
        => _client.GetAsync<Constellation>($"/universe/constellations/{constellationId}/", ct);

    public Task<List<long>?> GetSystemsAsync(CancellationToken ct = default)
        => _client.GetAsync<List<long>>("/universe/systems/", ct);

    public Task<SolarSystem?> GetSystemAsync(long systemId, CancellationToken ct = default)
        => _client.GetAsync<SolarSystem>($"/universe/systems/{systemId}/", ct);

    public Task<Star?> GetStarAsync(long starId, CancellationToken ct = default)
        => _client.GetAsync<Star>($"/universe/stars/{starId}/", ct);

    public Task<Station?> GetStationAsync(long stationId, CancellationToken ct = default)
        => _client.GetAsync<Station>($"/universe/stations/{stationId}/", ct);

    public Task<Stargate?> GetStargateAsync(long stargateId, CancellationToken ct = default)
        => _client.GetAsync<Stargate>($"/universe/stargates/{stargateId}/", ct);

    public Task<Planet?> GetPlanetAsync(long planetId, CancellationToken ct = default)
        => _client.GetAsync<Planet>($"/universe/planets/{planetId}/", ct);

    public Task<Moon?> GetMoonAsync(long moonId, CancellationToken ct = default)
        => _client.GetAsync<Moon>($"/universe/moons/{moonId}/", ct);

    public Task<EveType?> GetTypeAsync(long typeId, CancellationToken ct = default)
        => _client.GetAsync<EveType>($"/universe/types/{typeId}/", ct);

    public Task<ItemGroup?> GetGroupAsync(long groupId, CancellationToken ct = default)
        => _client.GetAsync<ItemGroup>($"/universe/groups/{groupId}/", ct);

    public Task<List<long>?> GetCategoriesAsync(CancellationToken ct = default)
        => _client.GetAsync<List<long>>("/universe/categories/", ct);

    public Task<ItemCategory?> GetCategoryAsync(long categoryId, CancellationToken ct = default)
        => _client.GetAsync<ItemCategory>($"/universe/categories/{categoryId}/", ct);

    public Task<List<Faction>?> GetFactionsAsync(CancellationToken ct = default)
        => _client.GetAsync<List<Faction>>("/universe/factions/", ct);

    public Task<List<Race>?> GetRacesAsync(CancellationToken ct = default)
        => _client.GetAsync<List<Race>>("/universe/races/", ct);

    public Task<List<Bloodline>?> GetBloodlinesAsync(CancellationToken ct = default)
        => _client.GetAsync<List<Bloodline>>("/universe/bloodlines/", ct);

    public Task<List<Ancestry>?> GetAncestriesAsync(CancellationToken ct = default)
        => _client.GetAsync<List<Ancestry>>("/universe/ancestries/", ct);

    public Task<List<SystemJumps>?> GetSystemJumpsAsync(CancellationToken ct = default)
        => _client.GetAsync<List<SystemJumps>>("/universe/system_jumps/", ct);

    public Task<List<SystemKills>?> GetSystemKillsAsync(CancellationToken ct = default)
        => _client.GetAsync<List<SystemKills>>("/universe/system_kills/", ct);
}
