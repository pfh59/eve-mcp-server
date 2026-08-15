using System.ComponentModel;
using eve_mcp_server.Services;
using ModelContextProtocol.Server;

namespace eve_mcp_server.Tools;

/// <summary>
/// MCP tools for routes, industry, sovereignty, dogma, and loyalty stores.
/// </summary>
[McpServerToolType]
public static class InfrastructureTools
{
    // ── Routes ───────────────────────────────────────────

    /// <summary>Calculate the shortest route between two systems.</summary>
    [McpServerTool(Name = "eve_get_route"), Description("Calculate the shortest route between two solar systems. Returns an ordered list of solar system IDs to travel through.")]
    public static Task<string> GetRoute(
        InfrastructureService svc,
        [Description("Origin solar system ID")] long originId,
        [Description("Destination solar system ID")] long destinationId,
        CancellationToken ct = default)
        => ToolRunner.RunAsync(() => svc.GetRouteAsync(originId, destinationId, ct), "Could not calculate route.");

    // ── Industry ─────────────────────────────────────────

    /// <summary>List public industry facilities.</summary>
    [McpServerTool(Name = "eve_get_industry_facilities"), Description("Get public industry facilities that can be used for manufacturing, research, etc.")]
    public static Task<string> GetIndustryFacilities(InfrastructureService svc, CancellationToken ct)
        => ToolRunner.RunAsync(() => svc.GetIndustryFacilitiesAsync(ct));

    /// <summary>Get cost indices for industry activities per system.</summary>
    [McpServerTool(Name = "eve_get_industry_systems"), Description("Get industry cost indices for all solar systems, showing the cost multiplier for each activity type.")]
    public static Task<string> GetIndustrySystems(InfrastructureService svc, CancellationToken ct)
        => ToolRunner.RunAsync(() => svc.GetIndustrySystemsAsync(ct));

    // ── Sovereignty ──────────────────────────────────────

    /// <summary>Get the sovereignty map.</summary>
    [McpServerTool(Name = "eve_get_sovereignty_map"), Description("Get the sovereignty map showing which alliance/corporation/faction owns each solar system.")]
    public static Task<string> GetSovereigntyMap(InfrastructureService svc, CancellationToken ct)
        => ToolRunner.RunAsync(() => svc.GetSovereigntyMapAsync(ct));

    /// <summary>Get sovereignty structures with vulnerability timers.</summary>
    [McpServerTool(Name = "eve_get_sovereignty_structures"), Description("Get sovereignty structures (TCUs, IHubs) including vulnerability timers.")]
    public static Task<string> GetSovereigntyStructures(InfrastructureService svc, CancellationToken ct)
        => ToolRunner.RunAsync(() => svc.GetSovereigntyStructuresAsync(ct));

    // ── Dogma ────────────────────────────────────────────

    /// <summary>List all dogma attribute IDs.</summary>
    [McpServerTool(Name = "eve_get_dogma_attributes"), Description("Get a list of all dogma attribute IDs. Dogma attributes define item properties like damage, speed, etc.")]
    public static Task<string> GetDogmaAttributes(InfrastructureService svc, CancellationToken ct)
        => ToolRunner.RunAsync(() => svc.GetDogmaAttributesAsync(ct));

    /// <summary>Get details for a dogma attribute.</summary>
    [McpServerTool(Name = "eve_get_dogma_attribute"), Description("Get details about a specific dogma attribute (name, description, default value, high/low is good, etc.).")]
    public static Task<string> GetDogmaAttribute(
        InfrastructureService svc,
        [Description("The dogma attribute ID")] long attributeId,
        CancellationToken ct = default)
        => ToolRunner.RunAsync(() => svc.GetDogmaAttributeAsync(attributeId, ct), "Dogma attribute not found.");

    /// <summary>List all dogma effect IDs.</summary>
    [McpServerTool(Name = "eve_get_dogma_effects"), Description("Get a list of all dogma effect IDs. Dogma effects define special abilities and behaviors.")]
    public static Task<string> GetDogmaEffects(InfrastructureService svc, CancellationToken ct)
        => ToolRunner.RunAsync(() => svc.GetDogmaEffectsAsync(ct));

    /// <summary>Get details for a dogma effect.</summary>
    [McpServerTool(Name = "eve_get_dogma_effect"), Description("Get details about a specific dogma effect.")]
    public static Task<string> GetDogmaEffect(
        InfrastructureService svc,
        [Description("The dogma effect ID")] long effectId,
        CancellationToken ct = default)
        => ToolRunner.RunAsync(() => svc.GetDogmaEffectAsync(effectId, ct), "Dogma effect not found.");

    // ── Loyalty Store ────────────────────────────────────

    /// <summary>Get loyalty store offers for a corporation.</summary>
    [McpServerTool(Name = "eve_get_loyalty_store_offers"), Description("Get loyalty store offers for a specific corporation, including required LP, ISK, and items.")]
    public static Task<string> GetLoyaltyStoreOffers(
        InfrastructureService svc,
        [Description("The corporation ID")] long corporationId,
        CancellationToken ct = default)
        => ToolRunner.RunAsync(() => svc.GetLoyaltyStoreOffersAsync(corporationId, ct));
}
