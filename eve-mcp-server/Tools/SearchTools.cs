using System.ComponentModel;
using eve_mcp_server.Services;
using ModelContextProtocol.Server;

namespace eve_mcp_server.Tools;

/// <summary>
/// MCP tools for resolving EVE Online names ↔ IDs.
/// </summary>
[McpServerToolType]
public static class SearchTools
{
    /// <summary>ESI caps /universe/ids/ at 500 names per request.</summary>
    private const int MaxNames = 500;

    /// <summary>ESI caps /universe/names/ at 1000 IDs per request.</summary>
    private const int MaxIds = 1000;

    [McpServerTool(Name = "eve_resolve_names_to_ids"), Description("Resolve EVE Online names to their IDs. Provide one or more names (characters, corporations, alliances, types, systems, regions, etc.) and get back the matching IDs grouped by category. Only exact matches are returned.")]
    public static Task<string> ResolveNamesToIds(
        SearchService svc,
        [Description("Comma-separated names to resolve (e.g., 'Jita,Tritanium,CCP Games')")] string names,
        CancellationToken ct = default)
    {
        var nameList = names.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList();
        if (nameList.Count > MaxNames)
            return Task.FromResult($"Too many names ({nameList.Count}). ESI accepts at most {MaxNames} per request.");

        return ToolRunner.RunAsync(() => svc.ResolveNamesToIdsAsync(nameList, ct), "No matches found.");
    }

    [McpServerTool(Name = "eve_resolve_ids_to_names"), Description("Resolve EVE Online IDs to their names and categories. Give one or more IDs and get back the name and category (character, corporation, alliance, type, system, etc.).")]
    public static Task<string> ResolveIdsToNames(
        SearchService svc,
        [Description("Comma-separated IDs to resolve (e.g., '30000142,34,98000001')")] string ids,
        CancellationToken ct = default)
    {
        var parts = ids.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts.Length > MaxIds)
            return Task.FromResult($"Too many IDs ({parts.Length}). ESI accepts at most {MaxIds} per request.");

        var idList = new List<long>(parts.Length);
        foreach (var part in parts)
        {
            if (!long.TryParse(part, out var id))
                return Task.FromResult($"Invalid ID value: '{part}'. All values must be numeric.");
            idList.Add(id);
        }
        return ToolRunner.RunAsync(() => svc.ResolveIdsToNamesAsync(idList, ct));
    }
}
