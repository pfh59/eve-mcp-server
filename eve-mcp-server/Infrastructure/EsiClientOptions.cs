namespace eve_mcp_server.Infrastructure;

/// <summary>
/// Configuration options for the ESI HTTP client.
/// </summary>
public sealed class EsiClientOptions
{
    /// <summary>Base URL for the ESI API.</summary>
    public string BaseUrl { get; set; } = "https://esi.evetech.net/latest";

    /// <summary>
    /// User-Agent string sent with every request.
    /// ESI best practices require identifying your application.
    /// </summary>
    public string UserAgent { get; set; } = "eve-mcp-server/1.0.0 (+https://github.com/eve-mcp-server)";

    /// <summary>Data source (tranquility or singularity).</summary>
    public string Datasource { get; set; } = "tranquility";
}
