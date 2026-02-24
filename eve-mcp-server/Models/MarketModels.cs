using System.Text.Json.Serialization;

namespace eve_mcp_server.Models;

// ═══════════════════════════════════════════
// Market Models
// ═══════════════════════════════════════════

public sealed class MarketGroup
{
    [JsonPropertyName("market_group_id")]
    public long MarketGroupId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("parent_group_id")]
    public long? ParentGroupId { get; set; }

    [JsonPropertyName("types")]
    public List<long> Types { get; set; } = [];
}

public sealed class MarketPrice
{
    [JsonPropertyName("type_id")]
    public long TypeId { get; set; }

    [JsonPropertyName("average_price")]
    public double? AveragePrice { get; set; }

    [JsonPropertyName("adjusted_price")]
    public double? AdjustedPrice { get; set; }
}

public sealed class MarketOrder
{
    [JsonPropertyName("order_id")]
    public long OrderId { get; set; }

    [JsonPropertyName("type_id")]
    public long TypeId { get; set; }

    [JsonPropertyName("location_id")]
    public long LocationId { get; set; }

    [JsonPropertyName("system_id")]
    public long SystemId { get; set; }

    [JsonPropertyName("volume_total")]
    public long VolumeTotal { get; set; }

    [JsonPropertyName("volume_remain")]
    public long VolumeRemain { get; set; }

    [JsonPropertyName("min_volume")]
    public long MinVolume { get; set; }

    [JsonPropertyName("price")]
    public double Price { get; set; }

    [JsonPropertyName("is_buy_order")]
    public bool IsBuyOrder { get; set; }

    [JsonPropertyName("duration")]
    public int Duration { get; set; }

    [JsonPropertyName("issued")]
    public DateTime Issued { get; set; }

    [JsonPropertyName("range")]
    public string Range { get; set; } = string.Empty;
}

public sealed class MarketHistory
{
    [JsonPropertyName("date")]
    public string Date { get; set; } = string.Empty;

    [JsonPropertyName("order_count")]
    public long OrderCount { get; set; }

    [JsonPropertyName("volume")]
    public long Volume { get; set; }

    [JsonPropertyName("highest")]
    public double Highest { get; set; }

    [JsonPropertyName("lowest")]
    public double Lowest { get; set; }

    [JsonPropertyName("average")]
    public double Average { get; set; }
}
