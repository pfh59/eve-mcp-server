using System.Text.Json.Serialization;

namespace eve_mcp_server.Models;

// ═══════════════════════════════════════════
// Universe Models
// ═══════════════════════════════════════════

public sealed class Region
{
    [JsonPropertyName("region_id")]
    public long RegionId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("constellations")]
    public List<long> Constellations { get; set; } = [];
}

public sealed class Constellation
{
    [JsonPropertyName("constellation_id")]
    public long ConstellationId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("region_id")]
    public long RegionId { get; set; }

    [JsonPropertyName("systems")]
    public List<long> Systems { get; set; } = [];

    [JsonPropertyName("position")]
    public Position? Position { get; set; }
}

public sealed class SolarSystem
{
    [JsonPropertyName("system_id")]
    public long SystemId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("constellation_id")]
    public long ConstellationId { get; set; }

    [JsonPropertyName("security_status")]
    public double SecurityStatus { get; set; }

    [JsonPropertyName("security_class")]
    public string? SecurityClass { get; set; }

    [JsonPropertyName("star_id")]
    public long? StarId { get; set; }

    [JsonPropertyName("stargates")]
    public List<long>? Stargates { get; set; }

    [JsonPropertyName("stations")]
    public List<long>? Stations { get; set; }

    [JsonPropertyName("planets")]
    public List<SystemPlanet>? Planets { get; set; }

    [JsonPropertyName("position")]
    public Position? Position { get; set; }
}

public sealed class SystemPlanet
{
    [JsonPropertyName("planet_id")]
    public long PlanetId { get; set; }

    [JsonPropertyName("asteroid_belts")]
    public List<long>? AsteroidBelts { get; set; }

    [JsonPropertyName("moons")]
    public List<long>? Moons { get; set; }
}

public sealed class Position
{
    [JsonPropertyName("x")]
    public double X { get; set; }

    [JsonPropertyName("y")]
    public double Y { get; set; }

    [JsonPropertyName("z")]
    public double Z { get; set; }
}

public sealed class Star
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("type_id")]
    public long TypeId { get; set; }

    [JsonPropertyName("age")]
    public long Age { get; set; }

    [JsonPropertyName("luminosity")]
    public double Luminosity { get; set; }

    [JsonPropertyName("radius")]
    public long Radius { get; set; }

    [JsonPropertyName("spectral_class")]
    public string? SpectralClass { get; set; }

    [JsonPropertyName("temperature")]
    public long Temperature { get; set; }
}

public sealed class Station
{
    [JsonPropertyName("station_id")]
    public long StationId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("system_id")]
    public long SystemId { get; set; }

    [JsonPropertyName("type_id")]
    public long TypeId { get; set; }

    [JsonPropertyName("owner")]
    public long? Owner { get; set; }

    [JsonPropertyName("race_id")]
    public long? RaceId { get; set; }

    [JsonPropertyName("max_dockable_ship_volume")]
    public double? MaxDockableShipVolume { get; set; }

    [JsonPropertyName("office_rental_cost")]
    public double? OfficeRentalCost { get; set; }

    [JsonPropertyName("reprocessing_efficiency")]
    public double? ReprocessingEfficiency { get; set; }

    [JsonPropertyName("reprocessing_stations_take")]
    public double? ReprocessingStationsTake { get; set; }

    [JsonPropertyName("services")]
    public List<string>? Services { get; set; }

    [JsonPropertyName("position")]
    public Position? Position { get; set; }
}

public sealed class Stargate
{
    [JsonPropertyName("stargate_id")]
    public long StargateId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("system_id")]
    public long SystemId { get; set; }

    [JsonPropertyName("type_id")]
    public long TypeId { get; set; }

    [JsonPropertyName("destination")]
    public StargateDestination? Destination { get; set; }

    [JsonPropertyName("position")]
    public Position? Position { get; set; }
}

public sealed class StargateDestination
{
    [JsonPropertyName("stargate_id")]
    public long StargateId { get; set; }

    [JsonPropertyName("system_id")]
    public long SystemId { get; set; }
}

public sealed class Planet
{
    [JsonPropertyName("planet_id")]
    public long PlanetId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("system_id")]
    public long SystemId { get; set; }

    [JsonPropertyName("type_id")]
    public long TypeId { get; set; }

    [JsonPropertyName("position")]
    public Position? Position { get; set; }
}

public sealed class Moon
{
    [JsonPropertyName("moon_id")]
    public long MoonId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("system_id")]
    public long SystemId { get; set; }

    [JsonPropertyName("position")]
    public Position? Position { get; set; }
}

public sealed class EveType
{
    [JsonPropertyName("type_id")]
    public long TypeId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("group_id")]
    public long GroupId { get; set; }

    [JsonPropertyName("published")]
    public bool Published { get; set; }

    [JsonPropertyName("market_group_id")]
    public long? MarketGroupId { get; set; }

    [JsonPropertyName("mass")]
    public double? Mass { get; set; }

    [JsonPropertyName("volume")]
    public double? Volume { get; set; }

    [JsonPropertyName("capacity")]
    public double? Capacity { get; set; }

    [JsonPropertyName("portion_size")]
    public long? PortionSize { get; set; }

    [JsonPropertyName("radius")]
    public double? Radius { get; set; }

    [JsonPropertyName("icon_id")]
    public long? IconId { get; set; }

    [JsonPropertyName("graphic_id")]
    public long? GraphicId { get; set; }

    [JsonPropertyName("dogma_attributes")]
    public List<DogmaAttribute>? DogmaAttributes { get; set; }

    [JsonPropertyName("dogma_effects")]
    public List<DogmaEffect>? DogmaEffects { get; set; }
}

public sealed class DogmaAttribute
{
    [JsonPropertyName("attribute_id")]
    public long AttributeId { get; set; }

    [JsonPropertyName("value")]
    public double Value { get; set; }
}

public sealed class DogmaEffect
{
    [JsonPropertyName("effect_id")]
    public long EffectId { get; set; }

    [JsonPropertyName("is_default")]
    public bool IsDefault { get; set; }
}

public sealed class ItemGroup
{
    [JsonPropertyName("group_id")]
    public long GroupId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("category_id")]
    public long CategoryId { get; set; }

    [JsonPropertyName("published")]
    public bool Published { get; set; }

    [JsonPropertyName("types")]
    public List<long> Types { get; set; } = [];
}

public sealed class ItemCategory
{
    [JsonPropertyName("category_id")]
    public long CategoryId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("published")]
    public bool Published { get; set; }

    [JsonPropertyName("groups")]
    public List<long> Groups { get; set; } = [];
}

public sealed class Faction
{
    [JsonPropertyName("faction_id")]
    public long FactionId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("corporation_id")]
    public long? CorporationId { get; set; }

    [JsonPropertyName("militia_corporation_id")]
    public long? MilitiaCorporationId { get; set; }

    [JsonPropertyName("solar_system_id")]
    public long? SolarSystemId { get; set; }

    [JsonPropertyName("station_count")]
    public int? StationCount { get; set; }

    [JsonPropertyName("station_system_count")]
    public int? StationSystemCount { get; set; }

    [JsonPropertyName("size_factor")]
    public double? SizeFactor { get; set; }

    [JsonPropertyName("is_unique")]
    public bool IsUnique { get; set; }
}

public sealed class Race
{
    [JsonPropertyName("race_id")]
    public long RaceId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("alliance_id")]
    public long? AllianceId { get; set; }
}

public sealed class Ancestry
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("bloodline_id")]
    public long BloodlineId { get; set; }

    [JsonPropertyName("short_description")]
    public string? ShortDescription { get; set; }

    [JsonPropertyName("icon_id")]
    public long? IconId { get; set; }
}

public sealed class Bloodline
{
    [JsonPropertyName("bloodline_id")]
    public long BloodlineId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("race_id")]
    public long RaceId { get; set; }

    [JsonPropertyName("corporation_id")]
    public long CorporationId { get; set; }

    [JsonPropertyName("ship_type_id")]
    public long ShipTypeId { get; set; }

    [JsonPropertyName("charisma")]
    public int Charisma { get; set; }

    [JsonPropertyName("intelligence")]
    public int Intelligence { get; set; }

    [JsonPropertyName("memory")]
    public int Memory { get; set; }

    [JsonPropertyName("perception")]
    public int Perception { get; set; }

    [JsonPropertyName("willpower")]
    public int Willpower { get; set; }
}
