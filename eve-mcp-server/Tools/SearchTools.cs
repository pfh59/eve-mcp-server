using System.ComponentModel;
using System.Text.Json;
using eve_mcp_server.Services;
using ModelContextProtocol.Server;

namespace eve_mcp_server.Tools;

/// <summary>
/// MCP tools for resolving EVE Online names ↔ IDs.
/// </summary>
[McpServerToolType]
public static class SearchTools
{
    /// <summary>Resolve one or more names to their ESI IDs (exact match).</summary>
    [McpServerTool, Description("Resolve EVE Online names to their IDs. Provide one or more names (characters, corporations, alliances, types, systems, regions, etc.) and get back the matching IDs grouped by category. Only exact matches are returned.")]
    public static async Task<string> ResolveNamesToIds(
        SearchService svc,
        [Description("Comma-separated names to resolve (e.g., 'Jita,Tritanium,CCP Games')")] string names,
        CancellationToken ct = default)
    {
        var nameList = names.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList();
        var result = await svc.ResolveNamesToIdsAsync(nameList, ct);
        return result is null ? "No matches found." : JsonSerializer.Serialize(result);
    }

    /// <summary>Resolve one or more ESI IDs to their names and categories.</summary>
    [McpServerTool, Description("Resolve EVE Online IDs to their names and categories. Give one or more IDs and get back the name and category (character, corporation, alliance, type, system, etc.).")]
    public static async Task<string> ResolveIdsToNames(
        SearchService svc,
        [Description("Comma-separated IDs to resolve (e.g., '30000142,34,98000001')")] string ids,
        CancellationToken ct = default)
    {
        var parts = ids.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var idList = new List<long>(parts.Length);
        foreach (var part in parts)
        {
            if (!long.TryParse(part, out var id))
                return $"Invalid ID value: '{part}'. All values must be numeric.";
            idList.Add(id);
        }
        var result = await svc.ResolveIdsToNamesAsync(idList, ct);
        return JsonSerializer.Serialize(result);
    }
}
