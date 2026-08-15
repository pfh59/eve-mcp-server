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

    public Task<List<long>?> GetRouteAsync(long originId, long destinationId, CancellationToken ct = default)
        => _client.GetAsync<List<long>>($"/route/{originId}/{destinationId}/", ct);

    public Task<List<IndustryFacility>?> GetIndustryFacilitiesAsync(CancellationToken ct = default)
        => _client.GetAsync<List<IndustryFacility>>("/industry/facilities/", ct);

    public Task<List<IndustrySystem>?> GetIndustrySystemsAsync(CancellationToken ct = default)
        => _client.GetAsync<List<IndustrySystem>>("/industry/systems/", ct);

    public Task<List<SovereigntyMap>?> GetSovereigntyMapAsync(CancellationToken ct = default)
        => _client.GetAsync<List<SovereigntyMap>>("/sovereignty/map/", ct);

    /// <summary>Sovereignty structures (TCUs, IHubs) with vulnerability timers.</summary>
    public Task<List<SovereigntyStructure>?> GetSovereigntyStructuresAsync(CancellationToken ct = default)
        => _client.GetAsync<List<SovereigntyStructure>>("/sovereignty/structures/", ct);

    public Task<List<long>?> GetDogmaAttributesAsync(CancellationToken ct = default)
        => _client.GetAsync<List<long>>("/dogma/attributes/", ct);

    public Task<DogmaAttributeDetail?> GetDogmaAttributeAsync(long attributeId, CancellationToken ct = default)
        => _client.GetAsync<DogmaAttributeDetail>($"/dogma/attributes/{attributeId}/", ct);

    public Task<List<long>?> GetDogmaEffectsAsync(CancellationToken ct = default)
        => _client.GetAsync<List<long>>("/dogma/effects/", ct);

    public Task<DogmaEffectDetail?> GetDogmaEffectAsync(long effectId, CancellationToken ct = default)
        => _client.GetAsync<DogmaEffectDetail>($"/dogma/effects/{effectId}/", ct);

    public Task<List<LoyaltyStoreOffer>?> GetLoyaltyStoreOffersAsync(long corporationId, CancellationToken ct = default)
        => _client.GetAsync<List<LoyaltyStoreOffer>>($"/loyalty/stores/{corporationId}/offers/", ct);
}
