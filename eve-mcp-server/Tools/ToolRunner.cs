using System.Text.Json;
using eve_mcp_server.Esi;

namespace eve_mcp_server.Tools;

/// <summary>
/// Shared execution wrapper for MCP tools: serializes results, maps a null
/// result (404) to a "not found" message, and converts ESI/validation errors
/// into readable tool output instead of raw exceptions. This keeps an ESI
/// outage from being reported to the model as "resource does not exist".
/// </summary>
internal static class ToolRunner
{
    public static async Task<string> RunAsync<T>(Func<Task<T?>> action, string? notFoundMessage = null)
    {
        try
        {
            var result = await action();
            if (result is null && notFoundMessage is not null)
                return notFoundMessage;
            return JsonSerializer.Serialize(result);
        }
        catch (EsiApiException ex)
        {
            return $"ESI request failed: {ex.Message}";
        }
        catch (ArgumentException ex)
        {
            return $"Invalid parameter: {ex.Message}";
        }
    }
}
