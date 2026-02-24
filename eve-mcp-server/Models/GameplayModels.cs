using System.Text.Json.Serialization;

namespace eve_mcp_server.Models;

// ═══════════════════════════════════════════
// Server Status, Killmails, Wars, Incursions
// ═══════════════════════════════════════════

public sealed class ServerStatus
{
    [JsonPropertyName("players")]
    public int Players { get; set; }

    [JsonPropertyName("server_version")]
    public string ServerVersion { get; set; } = string.Empty;

    [JsonPropertyName("start_time")]
    public DateTime StartTime { get; set; }

    [JsonPropertyName("vip")]
    public bool? Vip { get; set; }
}

public sealed class Killmail
{
    [JsonPropertyName("killmail_id")]
    public long KillmailId { get; set; }

    [JsonPropertyName("killmail_time")]
    public DateTime KillmailTime { get; set; }

    [JsonPropertyName("solar_system_id")]
    public long SolarSystemId { get; set; }

    [JsonPropertyName("victim")]
    public KillmailVictim? Victim { get; set; }

    [JsonPropertyName("attackers")]
    public List<KillmailAttacker>? Attackers { get; set; }
}

public sealed class KillmailVictim
{
    [JsonPropertyName("character_id")]
    public long? CharacterId { get; set; }

    [JsonPropertyName("corporation_id")]
    public long? CorporationId { get; set; }

    [JsonPropertyName("alliance_id")]
    public long? AllianceId { get; set; }

    [JsonPropertyName("faction_id")]
    public long? FactionId { get; set; }

    [JsonPropertyName("ship_type_id")]
    public long ShipTypeId { get; set; }

    [JsonPropertyName("damage_taken")]
    public long DamageTaken { get; set; }

    [JsonPropertyName("position")]
    public Position? Position { get; set; }

    [JsonPropertyName("items")]
    public List<KillmailItem>? Items { get; set; }
}

public sealed class KillmailItem
{
    [JsonPropertyName("item_type_id")]
    public long ItemTypeId { get; set; }

    [JsonPropertyName("quantity_destroyed")]
    public long? QuantityDestroyed { get; set; }

    [JsonPropertyName("quantity_dropped")]
    public long? QuantityDropped { get; set; }

    [JsonPropertyName("flag")]
    public int Flag { get; set; }

    [JsonPropertyName("singleton")]
    public int Singleton { get; set; }
}

public sealed class KillmailAttacker
{
    [JsonPropertyName("character_id")]
    public long? CharacterId { get; set; }

    [JsonPropertyName("corporation_id")]
    public long? CorporationId { get; set; }

    [JsonPropertyName("alliance_id")]
    public long? AllianceId { get; set; }

    [JsonPropertyName("faction_id")]
    public long? FactionId { get; set; }

    [JsonPropertyName("ship_type_id")]
    public long? ShipTypeId { get; set; }

    [JsonPropertyName("weapon_type_id")]
    public long? WeaponTypeId { get; set; }

    [JsonPropertyName("damage_done")]
    public long DamageDone { get; set; }

    [JsonPropertyName("final_blow")]
    public bool FinalBlow { get; set; }

    [JsonPropertyName("security_status")]
    public double SecurityStatus { get; set; }
}

public sealed class War
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("declared")]
    public DateTime Declared { get; set; }

    [JsonPropertyName("started")]
    public DateTime? Started { get; set; }

    [JsonPropertyName("finished")]
    public DateTime? Finished { get; set; }

    [JsonPropertyName("retracted")]
    public DateTime? Retracted { get; set; }

    [JsonPropertyName("mutual")]
    public bool Mutual { get; set; }

    [JsonPropertyName("open_for_allies")]
    public bool OpenForAllies { get; set; }

    [JsonPropertyName("aggressor")]
    public WarParty? Aggressor { get; set; }

    [JsonPropertyName("defender")]
    public WarParty? Defender { get; set; }
}

public sealed class WarParty
{
    [JsonPropertyName("alliance_id")]
    public long? AllianceId { get; set; }

    [JsonPropertyName("corporation_id")]
    public long? CorporationId { get; set; }

    [JsonPropertyName("isk_destroyed")]
    public double IskDestroyed { get; set; }

    [JsonPropertyName("ships_killed")]
    public long ShipsKilled { get; set; }
}

public sealed class WarKillmail
{
    [JsonPropertyName("killmail_id")]
    public long KillmailId { get; set; }

    [JsonPropertyName("killmail_hash")]
    public string KillmailHash { get; set; } = string.Empty;
}

public sealed class Incursion
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("state")]
    public string State { get; set; } = string.Empty;

    [JsonPropertyName("influence")]
    public double Influence { get; set; }

    [JsonPropertyName("has_boss")]
    public bool HasBoss { get; set; }

    [JsonPropertyName("faction_id")]
    public long FactionId { get; set; }

    [JsonPropertyName("constellation_id")]
    public long ConstellationId { get; set; }

    [JsonPropertyName("staging_solar_system_id")]
    public long StagingSolarSystemId { get; set; }

    [JsonPropertyName("infested_solar_systems")]
    public List<long> InfestedSolarSystems { get; set; } = [];
}
