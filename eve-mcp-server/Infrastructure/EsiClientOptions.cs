namespace eve_mcp_server.Infrastructure;

/// <summary>
/// Configuration options for the ESI HTTP client.
/// Bound from the "ESI" configuration section, so every value can be
/// overridden with environment variables (ESI__UserAgent, ESI__BaseUrl, ...).
/// </summary>
public sealed class EsiClientOptions
{
    /// <summary>
    /// Base URL for the ESI API. Routes are unversioned; API behavior is pinned
    /// with the X-Compatibility-Date header (see <see cref="CompatibilityDate"/>).
    /// </summary>
    public string BaseUrl { get; set; } = "https://esi.evetech.net";

    /// <summary>
    /// User-Agent string sent with every request. ESI best practices require
    /// identifying your application; CCP strongly prefers a reachable contact
    /// (email address) — set ESI__UserAgent to include yours.
    /// </summary>
    public string UserAgent { get; set; } = "eve-mcp-server/1.0.0 (+https://github.com/pfh59/eve-mcp-server)";

    /// <summary>Data source (tranquility or singularity).</summary>
    public string Datasource { get; set; } = "tranquility";

    /// <summary>
    /// ESI compatibility date (ISO yyyy-MM-dd): "give me the API behavior as it
    /// was at this date". Bump it after reviewing ESI changelogs.
    /// </summary>
    public string CompatibilityDate { get; set; } = "2026-08-15";

    /// <summary>Per-request HTTP timeout in seconds.</summary>
    public int TimeoutSeconds { get; set; } = 30;

    /// <summary>Upper bound on memory used by the response cache, in bytes.</summary>
    public long MaxCacheSizeBytes { get; set; } = 32 * 1024 * 1024;
}
