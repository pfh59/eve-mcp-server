using eve_mcp_server.Infrastructure;
using eve_mcp_server.Models;

namespace eve_mcp_server.Services;

/// <summary>
/// Service for routes, industry, sovereignty, dogma, and loyalty store endpoints.
/// </summary>
public sealed class InfrastructureService
{
    private readonly EsiClient _client;

    public InfrastructureService(EsiClient client) => _client = client;

    // ── Routes ───────────────────────────────────────────

    /// <summary>Calculate route between two solar systems.</summary>
    public Task<List<long>?> GetRouteAsync(long originId, long destinationId, CancellationToken ct = default)
        => _client.GetAsync<List<long>>($"/route/{originId}/{destinationId}/", ct);

    // ── Industry ─────────────────────────────────────────

    /// <summary>Get public industry facilities.</summary>
    public Task<List<IndustryFacility>?> GetIndustryFacilitiesAsync(CancellationToken ct = default)
        => _client.GetAsync<List<IndustryFacility>>("/industry/facilities/", ct);

    /// <summary>Get industry cost indices per solar system.</summary>
    public Task<List<IndustrySystem>?> GetIndustrySystemsAsync(CancellationToken ct = default)
        => _client.GetAsync<List<IndustrySystem>>("/industry/systems/", ct);

    // ── Sovereignty ──────────────────────────────────────

    /// <summary>Get sovereignty map (alliance/corporation/faction ownership per system).</summary>
    public Task<List<SovereigntyMap>?> GetSovereigntyMapAsync(CancellationToken ct = default)
        => _client.GetAsync<List<SovereigntyMap>>("/sovereignty/map/", ct);

    /// <summary>Get sovereignty structures (TCUs, IHubs).</summary>
    public Task<List<SovereigntyStructure>?> GetSovereigntyStructuresAsync(CancellationToken ct = default)
        => _client.GetAsync<List<SovereigntyStructure>>("/sovereignty/structures/", ct);

    // ── Dogma ────────────────────────────────────────────

    /// <summary>Get all dogma attribute IDs.</summary>
    public Task<List<long>?> GetDogmaAttributesAsync(CancellationToken ct = default)
        => _client.GetAsync<List<long>>("/dogma/attributes/", ct);

    /// <summary>Get details about a dogma attribute.</summary>
    public Task<DogmaAttributeDetail?> GetDogmaAttributeAsync(long attributeId, CancellationToken ct = default)
        => _client.GetAsync<DogmaAttributeDetail>($"/dogma/attributes/{attributeId}/", ct);

    /// <summary>Get all dogma effect IDs.</summary>
    public Task<List<long>?> GetDogmaEffectsAsync(CancellationToken ct = default)
        => _client.GetAsync<List<long>>("/dogma/effects/", ct);

    /// <summary>Get details about a dogma effect.</summary>
    public Task<DogmaEffectDetail?> GetDogmaEffectAsync(long effectId, CancellationToken ct = default)
        => _client.GetAsync<DogmaEffectDetail>($"/dogma/effects/{effectId}/", ct);

    // ── Loyalty Store ────────────────────────────────────

    /// <summary>Get loyalty store offers for a corporation.</summary>
    public Task<List<LoyaltyStoreOffer>?> GetLoyaltyStoreOffersAsync(long corporationId, CancellationToken ct = default)
        => _client.GetAsync<List<LoyaltyStoreOffer>>($"/loyalty/stores/{corporationId}/offers/", ct);
}
