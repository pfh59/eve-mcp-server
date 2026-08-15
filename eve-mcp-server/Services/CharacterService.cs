using eve_mcp_server.Infrastructure;
using eve_mcp_server.Models;

namespace eve_mcp_server.Services;

/// <summary>
/// Service for public Character, Corporation, and Alliance information.
/// </summary>
public sealed class CharacterService
{
    private readonly EsiClient _client;

    public CharacterService(EsiClient client) => _client = client;

    public Task<CharacterPublicInfo?> GetCharacterAsync(long characterId, CancellationToken ct = default)
        => _client.GetAsync<CharacterPublicInfo>($"/characters/{characterId}/", ct);

    /// <summary>Corporation/alliance/faction affiliations; ESI caps the batch at 1000 IDs.</summary>
    public Task<List<CharacterAffiliation>?> GetAffiliationsAsync(List<long> characterIds, CancellationToken ct = default)
        => _client.PostAsync<List<CharacterAffiliation>>("/characters/affiliation/", characterIds, ct);

    public Task<CorporationPublicInfo?> GetCorporationAsync(long corporationId, CancellationToken ct = default)
        => _client.GetAsync<CorporationPublicInfo>($"/corporations/{corporationId}/", ct);

    public Task<List<long>?> GetAlliancesAsync(CancellationToken ct = default)
        => _client.GetAsync<List<long>>("/alliances/", ct);

    public Task<AlliancePublicInfo?> GetAllianceAsync(long allianceId, CancellationToken ct = default)
        => _client.GetAsync<AlliancePublicInfo>($"/alliances/{allianceId}/", ct);

    public Task<List<long>?> GetAllianceCorporationsAsync(long allianceId, CancellationToken ct = default)
        => _client.GetAsync<List<long>>($"/alliances/{allianceId}/corporations/", ct);
}
