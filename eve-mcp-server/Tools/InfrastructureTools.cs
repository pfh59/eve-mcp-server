using System.ComponentModel;
using System.Text.Json;
using eve_mcp_server.Services;
using ModelContextProtocol.Server;

namespace eve_mcp_server.Tools;

[McpServerToolType]
public static class InfrastructureTools
{
    // ── Routes ───────────────────────────────────────────

    [McpServerTool, Description("Calculate the shortest route between two solar systems. Returns an ordered list of solar system IDs to travel through.")]
    public static async Task<string> GetRoute(
        InfrastructureService svc,
        [Description("Origin solar system ID")] long originId,
        [Description("Destination solar system ID")] long destinationId,
        CancellationToken ct = default)
    {
        var result = await svc.GetRouteAsync(originId, destinationId, ct);
        return result is null ? "Could not calculate route." : JsonSerializer.Serialize(result);
    }

    // ── Industry ─────────────────────────────────────────

    [McpServerTool, Description("Get public industry facilities that can be used for manufacturing, research, etc.")]
    public static async Task<string> GetIndustryFacilities(InfrastructureService svc, CancellationToken ct)
    {
        var result = await svc.GetIndustryFacilitiesAsync(ct);
        return JsonSerializer.Serialize(result);
    }

    [McpServerTool, Description("Get industry cost indices for all solar systems, showing the cost multiplier for each activity type.")]
    public static async Task<string> GetIndustrySystems(InfrastructureService svc, CancellationToken ct)
    {
        var result = await svc.GetIndustrySystemsAsync(ct);
        return JsonSerializer.Serialize(result);
    }

    // ── Sovereignty ──────────────────────────────────────

    [McpServerTool, Description("Get the sovereignty map showing which alliance/corporation/faction owns each solar system.")]
    public static async Task<string> GetSovereigntyMap(InfrastructureService svc, CancellationToken ct)
    {
        var result = await svc.GetSovereigntyMapAsync(ct);
        return JsonSerializer.Serialize(result);
    }

    [McpServerTool, Description("Get sovereignty structures (TCUs, IHubs) including vulnerability timers.")]
    public static async Task<string> GetSovereigntyStructures(InfrastructureService svc, CancellationToken ct)
    {
        var result = await svc.GetSovereigntyStructuresAsync(ct);
        return JsonSerializer.Serialize(result);
    }

    // ── Dogma ────────────────────────────────────────────

    [McpServerTool, Description("Get a list of all dogma attribute IDs. Dogma attributes define item properties like damage, speed, etc.")]
    public static async Task<string> GetDogmaAttributes(InfrastructureService svc, CancellationToken ct)
    {
        var result = await svc.GetDogmaAttributesAsync(ct);
        return JsonSerializer.Serialize(result);
    }

    [McpServerTool, Description("Get details about a specific dogma attribute (name, description, default value, high/low is good, etc.).")]
    public static async Task<string> GetDogmaAttribute(
        InfrastructureService svc,
        [Description("The dogma attribute ID")] long attributeId,
        CancellationToken ct = default)
    {
        var result = await svc.GetDogmaAttributeAsync(attributeId, ct);
        return result is null ? "Dogma attribute not found." : JsonSerializer.Serialize(result);
    }

    [McpServerTool, Description("Get a list of all dogma effect IDs. Dogma effects define special abilities and behaviors.")]
    public static async Task<string> GetDogmaEffects(InfrastructureService svc, CancellationToken ct)
    {
        var result = await svc.GetDogmaEffectsAsync(ct);
        return JsonSerializer.Serialize(result);
    }

    [McpServerTool, Description("Get details about a specific dogma effect.")]
    public static async Task<string> GetDogmaEffect(
        InfrastructureService svc,
        [Description("The dogma effect ID")] long effectId,
        CancellationToken ct = default)
    {
        var result = await svc.GetDogmaEffectAsync(effectId, ct);
        return result is null ? "Dogma effect not found." : JsonSerializer.Serialize(result);
    }

    // ── Loyalty Store ────────────────────────────────────

    [McpServerTool, Description("Get loyalty store offers for a specific corporation, including required LP, ISK, and items.")]
    public static async Task<string> GetLoyaltyStoreOffers(
        InfrastructureService svc,
        [Description("The corporation ID")] long corporationId,
        CancellationToken ct = default)
    {
        var result = await svc.GetLoyaltyStoreOffersAsync(corporationId, ct);
        return JsonSerializer.Serialize(result);
    }
}
