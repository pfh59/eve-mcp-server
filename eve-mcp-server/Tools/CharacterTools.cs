using System.ComponentModel;
using System.Text.Json;
using eve_mcp_server.Services;
using ModelContextProtocol.Server;

namespace eve_mcp_server.Tools;

/// <summary>
/// MCP tools for character, corporation, and alliance public information.
/// </summary>
[McpServerToolType]
public static class CharacterTools
{
    /// <summary>Get public info for a single character.</summary>
    [McpServerTool, Description("Get public information about an EVE character (name, birthday, corporation, description, etc.).")]
    public static async Task<string> GetCharacter(
        CharacterService svc,
        [Description("The character ID")] long characterId,
        CancellationToken ct = default)
    {
        var result = await svc.GetCharacterAsync(characterId, ct);
        return result is null ? "Character not found." : JsonSerializer.Serialize(result);
    }

    /// <summary>Get affiliations (corp, alliance, faction) for up to 1000 character IDs.</summary>
    [McpServerTool, Description("Get character affiliations (corporation, alliance, faction) for one or more character IDs. Accepts up to 1000 IDs.")]
    public static async Task<string> GetCharacterAffiliations(
        CharacterService svc,
        [Description("Comma-separated list of character IDs (up to 1000)")] string characterIds,
        CancellationToken ct = default)
    {
        var parts = characterIds.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var ids = new List<long>(parts.Length);
        foreach (var part in parts)
        {
            if (!long.TryParse(part, out var id))
                return $"Invalid character ID: '{part}'. All values must be numeric.";
            ids.Add(id);
        }
        var result = await svc.GetAffiliationsAsync(ids, ct);
        return JsonSerializer.Serialize(result);
    }

    /// <summary>Get public info for a corporation.</summary>
    [McpServerTool, Description("Get public information about a corporation (name, ticker, member count, CEO, description, etc.).")]
    public static async Task<string> GetCorporation(
        CharacterService svc,
        [Description("The corporation ID")] long corporationId,
        CancellationToken ct = default)
    {
        var result = await svc.GetCorporationAsync(corporationId, ct);
        return result is null ? "Corporation not found." : JsonSerializer.Serialize(result);
    }

    /// <summary>Get all alliance IDs.</summary>
    [McpServerTool, Description("Get a list of all alliance IDs in EVE Online.")]
    public static async Task<string> GetAlliances(CharacterService svc, CancellationToken ct)
    {
        var result = await svc.GetAlliancesAsync(ct);
        return JsonSerializer.Serialize(result);
    }

    /// <summary>Get public info for a single alliance.</summary>
    [McpServerTool, Description("Get public information about an alliance (name, ticker, founding date, executor corporation).")]
    public static async Task<string> GetAlliance(
        CharacterService svc,
        [Description("The alliance ID")] long allianceId,
        CancellationToken ct = default)
    {
        var result = await svc.GetAllianceAsync(allianceId, ct);
        return result is null ? "Alliance not found." : JsonSerializer.Serialize(result);
    }

    /// <summary>Get corporation members of an alliance.</summary>
    [McpServerTool, Description("Get the list of corporation IDs that are members of an alliance.")]
    public static async Task<string> GetAllianceCorporations(
        CharacterService svc,
        [Description("The alliance ID")] long allianceId,
        CancellationToken ct = default)
    {
        var result = await svc.GetAllianceCorporationsAsync(allianceId, ct);
        return JsonSerializer.Serialize(result);
    }
}
