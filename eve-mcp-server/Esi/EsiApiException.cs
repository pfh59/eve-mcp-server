namespace eve_mcp_server.Esi;

/// <summary>
/// Raised when ESI returns a non-recoverable error (4xx other than 404, 5xx,
/// exhausted rate-limit retries, or a network failure). The message is safe to
/// surface to the MCP client as-is.
/// </summary>
public sealed class EsiApiException : Exception
{
    /// <summary>HTTP status code returned by ESI, or 0 for network-level failures.</summary>
    public int StatusCode { get; }

    public EsiApiException(int statusCode, string message) : base(message)
        => StatusCode = statusCode;
}
