using System.Text.Json.Serialization;

namespace eve_mcp_server.Models;

// ═══════════════════════════════════════════
// Route, Industry, Sovereignty, Insurance
// ═══════════════════════════════════════════

public sealed class RouteResult
{
    [JsonPropertyName("route")]
    public List<long> Route { get; set; } = [];
}

public sealed class IndustryFacility
{
    [JsonPropertyName("facility_id")]
    public long FacilityId { get; set; }

    [JsonPropertyName("owner_id")]
    public long OwnerId { get; set; }

    [JsonPropertyName("region_id")]
    public long RegionId { get; set; }

    [JsonPropertyName("solar_system_id")]
    public long SolarSystemId { get; set; }

    [JsonPropertyName("type_id")]
    public long TypeId { get; set; }

    [JsonPropertyName("tax")]
    public double? Tax { get; set; }
}

public sealed class IndustrySystem
{
    [JsonPropertyName("solar_system_id")]
    public long SolarSystemId { get; set; }

    [JsonPropertyName("cost_indices")]
    public List<CostIndex> CostIndices { get; set; } = [];
}

public sealed class CostIndex
{
    [JsonPropertyName("activity")]
    public string Activity { get; set; } = string.Empty;

    [JsonPropertyName("cost_index")]
    public double CostIndexValue { get; set; }
}

public sealed class SovereigntyMap
{
    [JsonPropertyName("system_id")]
    public long SystemId { get; set; }

    [JsonPropertyName("alliance_id")]
    public long? AllianceId { get; set; }

    [JsonPropertyName("corporation_id")]
    public long? CorporationId { get; set; }

    [JsonPropertyName("faction_id")]
    public long? FactionId { get; set; }
}

public sealed class SovereigntyStructure
{
    [JsonPropertyName("alliance_id")]
    public long AllianceId { get; set; }

    [JsonPropertyName("solar_system_id")]
    public long SolarSystemId { get; set; }

    [JsonPropertyName("structure_id")]
    public long StructureId { get; set; }

    [JsonPropertyName("structure_type_id")]
    public long StructureTypeId { get; set; }

    [JsonPropertyName("vulnerability_occupancy_level")]
    public double? VulnerabilityOccupancyLevel { get; set; }
}

public sealed class InsurancePrice
{
    [JsonPropertyName("type_id")]
    public long TypeId { get; set; }

    [JsonPropertyName("levels")]
    public List<InsuranceLevel> Levels { get; set; } = [];
}

public sealed class InsuranceLevel
{
    [JsonPropertyName("cost")]
    public double Cost { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("payout")]
    public double Payout { get; set; }
}

public sealed class SystemJumps
{
    [JsonPropertyName("system_id")]
    public long SystemId { get; set; }

    [JsonPropertyName("ship_jumps")]
    public long ShipJumps { get; set; }
}

public sealed class SystemKills
{
    [JsonPropertyName("system_id")]
    public long SystemId { get; set; }

    [JsonPropertyName("ship_kills")]
    public long ShipKills { get; set; }

    [JsonPropertyName("npc_kills")]
    public long NpcKills { get; set; }

    [JsonPropertyName("pod_kills")]
    public long PodKills { get; set; }
}

// ═══════════════════════════════════════════
// Dogma Models
// ═══════════════════════════════════════════

public sealed class DogmaAttributeDetail
{
    [JsonPropertyName("attribute_id")]
    public long AttributeId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("display_name")]
    public string? DisplayName { get; set; }

    [JsonPropertyName("default_value")]
    public double? DefaultValue { get; set; }

    [JsonPropertyName("high_is_good")]
    public bool? HighIsGood { get; set; }

    [JsonPropertyName("icon_id")]
    public long? IconId { get; set; }

    [JsonPropertyName("published")]
    public bool? Published { get; set; }

    [JsonPropertyName("stackable")]
    public bool? Stackable { get; set; }

    [JsonPropertyName("unit_id")]
    public long? UnitId { get; set; }
}

public sealed class DogmaEffectDetail
{
    [JsonPropertyName("effect_id")]
    public long EffectId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("display_name")]
    public string? DisplayName { get; set; }

    [JsonPropertyName("effect_category")]
    public int? EffectCategory { get; set; }

    [JsonPropertyName("is_assistance")]
    public bool? IsAssistance { get; set; }

    [JsonPropertyName("is_offensive")]
    public bool? IsOffensive { get; set; }

    [JsonPropertyName("is_warp_safe")]
    public bool? IsWarpSafe { get; set; }

    [JsonPropertyName("published")]
    public bool? Published { get; set; }

    [JsonPropertyName("icon_id")]
    public long? IconId { get; set; }

    [JsonPropertyName("discharge_attribute_id")]
    public long? DischargeAttributeId { get; set; }

    [JsonPropertyName("duration_attribute_id")]
    public long? DurationAttributeId { get; set; }

    [JsonPropertyName("falloff_attribute_id")]
    public long? FalloffAttributeId { get; set; }

    [JsonPropertyName("range_attribute_id")]
    public long? RangeAttributeId { get; set; }

    [JsonPropertyName("tracking_speed_attribute_id")]
    public long? TrackingSpeedAttributeId { get; set; }
}

// ═══════════════════════════════════════════
// Loyalty Store Models
// ═══════════════════════════════════════════

public sealed class LoyaltyStoreOffer
{
    [JsonPropertyName("offer_id")]
    public long OfferId { get; set; }

    [JsonPropertyName("type_id")]
    public long TypeId { get; set; }

    [JsonPropertyName("quantity")]
    public long Quantity { get; set; }

    [JsonPropertyName("lp_cost")]
    public long LpCost { get; set; }

    [JsonPropertyName("isk_cost")]
    public long IskCost { get; set; }

    [JsonPropertyName("ak_cost")]
    public long? AkCost { get; set; }

    [JsonPropertyName("required_items")]
    public List<RequiredItem> RequiredItems { get; set; } = [];
}

public sealed class RequiredItem
{
    [JsonPropertyName("type_id")]
    public long TypeId { get; set; }

    [JsonPropertyName("quantity")]
    public long Quantity { get; set; }
}
