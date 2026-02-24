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

    /// <summary>Get public info about a character.</summary>
    public Task<CharacterPublicInfo?> GetCharacterAsync(long characterId, CancellationToken ct = default)
        => _client.GetAsync<CharacterPublicInfo>($"/characters/{characterId}/", ct);

    /// <summary>Get character affiliations (corporation, alliance, faction) for up to 1000 characters.</summary>
    public Task<List<CharacterAffiliation>?> GetAffiliationsAsync(List<long> characterIds, CancellationToken ct = default)
        => _client.PostAsync<List<CharacterAffiliation>>("/characters/affiliation/", characterIds, ct);

    /// <summary>Get public info about a corporation.</summary>
    public Task<CorporationPublicInfo?> GetCorporationAsync(long corporationId, CancellationToken ct = default)
        => _client.GetAsync<CorporationPublicInfo>($"/corporations/{corporationId}/", ct);

    /// <summary>Get list of all alliances.</summary>
    public Task<List<long>?> GetAlliancesAsync(CancellationToken ct = default)
        => _client.GetAsync<List<long>>("/alliances/", ct);

    /// <summary>Get public info about an alliance.</summary>
    public Task<AlliancePublicInfo?> GetAllianceAsync(long allianceId, CancellationToken ct = default)
        => _client.GetAsync<AlliancePublicInfo>($"/alliances/{allianceId}/", ct);

    /// <summary>Get corporations in an alliance.</summary>
    public Task<List<long>?> GetAllianceCorporationsAsync(long allianceId, CancellationToken ct = default)
        => _client.GetAsync<List<long>>($"/alliances/{allianceId}/corporations/", ct);
}
