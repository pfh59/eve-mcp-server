using System.Text.Json.Serialization;

namespace eve_mcp_server.Models;

// ═══════════════════════════════════════════
// Character & Alliance Models (public info only)
// ═══════════════════════════════════════════

public sealed class CharacterPublicInfo
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("birthday")]
    public DateTime Birthday { get; set; }

    [JsonPropertyName("gender")]
    public string Gender { get; set; } = string.Empty;

    [JsonPropertyName("bloodline_id")]
    public long BloodlineId { get; set; }

    [JsonPropertyName("race_id")]
    public long RaceId { get; set; }

    [JsonPropertyName("corporation_id")]
    public long CorporationId { get; set; }

    [JsonPropertyName("alliance_id")]
    public long? AllianceId { get; set; }

    [JsonPropertyName("faction_id")]
    public long? FactionId { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("security_status")]
    public double? SecurityStatus { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }
}

public sealed class CorporationPublicInfo
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("ticker")]
    public string Ticker { get; set; } = string.Empty;

    [JsonPropertyName("member_count")]
    public int MemberCount { get; set; }

    [JsonPropertyName("ceo_id")]
    public long CeoId { get; set; }

    [JsonPropertyName("creator_id")]
    public long CreatorId { get; set; }

    [JsonPropertyName("alliance_id")]
    public long? AllianceId { get; set; }

    [JsonPropertyName("faction_id")]
    public long? FactionId { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("date_founded")]
    public DateTime? DateFounded { get; set; }

    [JsonPropertyName("home_station_id")]
    public long? HomeStationId { get; set; }

    [JsonPropertyName("tax_rate")]
    public double TaxRate { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }
}

public sealed class AlliancePublicInfo
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("ticker")]
    public string Ticker { get; set; } = string.Empty;

    [JsonPropertyName("creator_id")]
    public long CreatorId { get; set; }

    [JsonPropertyName("creator_corporation_id")]
    public long CreatorCorporationId { get; set; }

    [JsonPropertyName("executor_corporation_id")]
    public long? ExecutorCorporationId { get; set; }

    [JsonPropertyName("date_founded")]
    public DateTime DateFounded { get; set; }

    [JsonPropertyName("faction_id")]
    public long? FactionId { get; set; }
}

public sealed class CharacterAffiliation
{
    [JsonPropertyName("character_id")]
    public long CharacterId { get; set; }

    [JsonPropertyName("corporation_id")]
    public long CorporationId { get; set; }

    [JsonPropertyName("alliance_id")]
    public long? AllianceId { get; set; }

    [JsonPropertyName("faction_id")]
    public long? FactionId { get; set; }
}
