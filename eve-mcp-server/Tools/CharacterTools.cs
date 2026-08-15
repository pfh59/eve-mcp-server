using System.ComponentModel;
using eve_mcp_server.Services;
using ModelContextProtocol.Server;

namespace eve_mcp_server.Tools;

/// <summary>
/// MCP tools for character, corporation, and alliance public information.
/// </summary>
[McpServerToolType]
public static class CharacterTools
{
    /// <summary>ESI caps /characters/affiliation/ at 1000 IDs per request.</summary>
    private const int MaxAffiliationIds = 1000;

    /// <summary>Get public info for a single character.</summary>
    [McpServerTool(Name = "eve_get_character"), Description("Get public information about an EVE character (name, birthday, corporation, description, etc.).")]
    public static Task<string> GetCharacter(
        CharacterService svc,
        [Description("The character ID")] long characterId,
        CancellationToken ct = default)
        => ToolRunner.RunAsync(() => svc.GetCharacterAsync(characterId, ct), "Character not found.");

    /// <summary>Get affiliations (corp, alliance, faction) for up to 1000 character IDs.</summary>
    [McpServerTool(Name = "eve_get_character_affiliations"), Description("Get character affiliations (corporation, alliance, faction) for one or more character IDs. Accepts up to 1000 IDs.")]
    public static Task<string> GetCharacterAffiliations(
        CharacterService svc,
        [Description("Comma-separated list of character IDs (up to 1000)")] string characterIds,
        CancellationToken ct = default)
    {
        var parts = characterIds.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts.Length > MaxAffiliationIds)
            return Task.FromResult($"Too many character IDs ({parts.Length}). ESI accepts at most {MaxAffiliationIds} per request.");

        var ids = new List<long>(parts.Length);
        foreach (var part in parts)
        {
            if (!long.TryParse(part, out var id))
                return Task.FromResult($"Invalid character ID: '{part}'. All values must be numeric.");
            ids.Add(id);
        }
        return ToolRunner.RunAsync(() => svc.GetAffiliationsAsync(ids, ct));
    }

    /// <summary>Get public info for a corporation.</summary>
    [McpServerTool(Name = "eve_get_corporation"), Description("Get public information about a corporation (name, ticker, member count, CEO, description, etc.).")]
    public static Task<string> GetCorporation(
        CharacterService svc,
        [Description("The corporation ID")] long corporationId,
        CancellationToken ct = default)
        => ToolRunner.RunAsync(() => svc.GetCorporationAsync(corporationId, ct), "Corporation not found.");

    /// <summary>Get all alliance IDs.</summary>
    [McpServerTool(Name = "eve_get_alliances"), Description("Get a list of all alliance IDs in EVE Online.")]
    public static Task<string> GetAlliances(CharacterService svc, CancellationToken ct)
        => ToolRunner.RunAsync(() => svc.GetAlliancesAsync(ct));

    /// <summary>Get public info for a single alliance.</summary>
    [McpServerTool(Name = "eve_get_alliance"), Description("Get public information about an alliance (name, ticker, founding date, executor corporation).")]
    public static Task<string> GetAlliance(
        CharacterService svc,
        [Description("The alliance ID")] long allianceId,
        CancellationToken ct = default)
        => ToolRunner.RunAsync(() => svc.GetAllianceAsync(allianceId, ct), "Alliance not found.");

    /// <summary>Get corporation members of an alliance.</summary>
    [McpServerTool(Name = "eve_get_alliance_corporations"), Description("Get the list of corporation IDs that are members of an alliance.")]
    public static Task<string> GetAllianceCorporations(
        CharacterService svc,
        [Description("The alliance ID")] long allianceId,
        CancellationToken ct = default)
        => ToolRunner.RunAsync(() => svc.GetAllianceCorporationsAsync(allianceId, ct));
}
