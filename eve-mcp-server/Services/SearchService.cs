using eve_mcp_server.Esi;
using eve_mcp_server.Models;

namespace eve_mcp_server.Services;

/// <summary>
/// Service for resolving EVE names ↔ IDs.
/// </summary>
public sealed class SearchService
{
    private readonly EsiClient _client;

    public SearchService(EsiClient client) => _client = client;

    /// <summary>
    /// Resolve a set of names to IDs. Only exact matches are returned.
    /// Supported categories: agents, alliances, characters, constellations,
    /// corporations, factions, inventory_types, regions, stations, systems.
    /// </summary>
    public Task<UniverseIdsResult?> ResolveNamesToIdsAsync(List<string> names, CancellationToken ct = default)
        => _client.PostAsync<UniverseIdsResult>("/universe/ids/", names, ct);

    /// <summary>
    /// Resolve a set of IDs to names and categories.
    /// Supported: Characters, Corporations, Alliances, Stations, Solar Systems,
    /// Constellations, Regions, Types, Factions.
    /// </summary>
    public Task<List<UniverseName>?> ResolveIdsToNamesAsync(List<long> ids, CancellationToken ct = default)
        => _client.PostAsync<List<UniverseName>>("/universe/names/", ids, ct);
}
