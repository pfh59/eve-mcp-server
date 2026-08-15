using eve_mcp_server.Infrastructure;
using eve_mcp_server.Models;

namespace eve_mcp_server.Services;

/// <summary>
/// Service for server status, killmails, wars, and incursions.
/// </summary>
public sealed class GameplayService
{
    private readonly EsiClient _client;

    public GameplayService(EsiClient client) => _client = client;

    public Task<ServerStatus?> GetStatusAsync(CancellationToken ct = default)
        => _client.GetAsync<ServerStatus>("/status/", ct);

    public Task<Killmail?> GetKillmailAsync(long killmailId, string killmailHash, CancellationToken ct = default)
    {
        // Killmail hashes are 40-char hex (SHA-1); reject anything else before it reaches the URL
        if (!KillmailHashPattern.IsMatch(killmailHash))
            throw new ArgumentException("killmailHash must be a 40-character hexadecimal string.", nameof(killmailHash));
        return _client.GetAsync<Killmail>($"/killmails/{killmailId}/{killmailHash}/", ct);
    }

    private static readonly System.Text.RegularExpressions.Regex KillmailHashPattern =
        new("^[a-fA-F0-9]{40}$", System.Text.RegularExpressions.RegexOptions.Compiled);

    /// <summary>War IDs, most recent first; paginate with maxWarId.</summary>
    public Task<List<long>?> GetWarsAsync(long? maxWarId = null, CancellationToken ct = default)
    {
        var url = "/wars/";
        if (maxWarId.HasValue)
            url += $"?max_war_id={maxWarId.Value}";
        return _client.GetAsync<List<long>>(url, ct);
    }

    public Task<War?> GetWarAsync(long warId, CancellationToken ct = default)
        => _client.GetAsync<War>($"/wars/{warId}/", ct);

    public Task<List<WarKillmail>?> GetWarKillmailsAsync(long warId, int page = 1, CancellationToken ct = default)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(page, 1);
        return _client.GetAsync<List<WarKillmail>>($"/wars/{warId}/killmails/?page={page}", ct);
    }

    public Task<List<Incursion>?> GetIncursionsAsync(CancellationToken ct = default)
        => _client.GetAsync<List<Incursion>>("/incursions/", ct);

    public Task<List<InsurancePrice>?> GetInsurancePricesAsync(CancellationToken ct = default)
        => _client.GetAsync<List<InsurancePrice>>("/insurance/prices/", ct);
}
