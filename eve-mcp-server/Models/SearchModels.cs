using System.Text.Json.Serialization;

namespace eve_mcp_server.Models;

public sealed class UniverseIdsResult
{
    [JsonPropertyName("agents")]
    public List<IdNamePair>? Agents { get; set; }

    [JsonPropertyName("alliances")]
    public List<IdNamePair>? Alliances { get; set; }

    [JsonPropertyName("characters")]
    public List<IdNamePair>? Characters { get; set; }

    [JsonPropertyName("constellations")]
    public List<IdNamePair>? Constellations { get; set; }

    [JsonPropertyName("corporations")]
    public List<IdNamePair>? Corporations { get; set; }

    [JsonPropertyName("factions")]
    public List<IdNamePair>? Factions { get; set; }

    [JsonPropertyName("inventory_types")]
    public List<IdNamePair>? InventoryTypes { get; set; }

    [JsonPropertyName("regions")]
    public List<IdNamePair>? Regions { get; set; }

    [JsonPropertyName("stations")]
    public List<IdNamePair>? Stations { get; set; }

    [JsonPropertyName("systems")]
    public List<IdNamePair>? Systems { get; set; }
}

public sealed class IdNamePair
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

public sealed class UniverseName
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("category")]
    public string Category { get; set; } = string.Empty;
}
