using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace eve_mcp_server.Infrastructure;

/// <summary>
/// Central HTTP client for ESI following best practices:
/// - User-Agent and X-Compatibility-Date headers on every request
/// - Time-based caching honoring the Expires header (no request before expiry)
/// - ETag caching with If-None-Match, bounded in size (LRU eviction)
/// - Error limit tracking (X-ESI-Error-Limit headers) shared across all callers
/// - Bounded retry on 429 with capped Retry-After
/// Server errors and unexpected 4xx surface as <see cref="EsiApiException"/>;
/// only 404 maps to a null result ("not found").
/// </summary>
public sealed class EsiClient : IDisposable
{
    private const int MaxRetries = 2;
    private static readonly TimeSpan MaxRetryDelay = TimeSpan.FromSeconds(60);
    private static readonly TimeSpan CacheSlidingExpiration = TimeSpan.FromHours(6);

    private readonly HttpClient _httpClient;
    private readonly EsiClientOptions _options;
    private readonly ILogger<EsiClient> _logger;

    // Response cache: URL → (ETag, body, Expires). Bounded by MaxCacheSizeBytes.
    private readonly MemoryCache _cache;

    // ESI error-limit state (shared across concurrent tool calls)
    private readonly object _errorLimitLock = new();
    private int _errorLimitRemaining = 100;
    private DateTimeOffset _errorLimitReset = DateTimeOffset.MinValue;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private sealed record CachedResponse(string? ETag, string Body, DateTimeOffset? Expires);

    public EsiClient(HttpClient httpClient, EsiClientOptions options, ILogger<EsiClient> logger)
    {
        _httpClient = httpClient;
        _options = options;
        _logger = logger;

        _httpClient.BaseAddress = new Uri(_options.BaseUrl);
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(_options.UserAgent);
        _httpClient.DefaultRequestHeaders.Add("X-Compatibility-Date", _options.CompatibilityDate);

        _cache = new MemoryCache(new MemoryCacheOptions { SizeLimit = _options.MaxCacheSizeBytes });
    }

    /// <summary>
    /// Perform a GET request to the ESI API with caching and rate limit awareness.
    /// Returns null only for 404 (resource does not exist).
    /// </summary>
    public async Task<T?> GetAsync<T>(string path, CancellationToken ct = default)
    {
        var url = AppendDatasource(path);
        var cached = _cache.Get<CachedResponse>(url);

        // Honor Expires: ESI forbids re-requesting a resource before its cache expiry.
        if (cached?.Expires is { } expires && DateTimeOffset.UtcNow < expires)
        {
            _logger.LogDebug("ESI cache hit (fresh until {Expires}) for {Url}", expires, url);
            return JsonSerializer.Deserialize<T>(cached.Body, JsonOptions);
        }

        for (var attempt = 0; ; attempt++)
        {
            await WaitIfErrorLimitExhaustedAsync(ct);

            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            if (cached?.ETag is not null)
            {
                request.Headers.IfNoneMatch.ParseAdd(cached.ETag);
            }

            using var response = await SendAsync(request, url, ct);

            TrackErrorLimits(response);
            TrackRateLimits(response);

            // 304 Not Modified → return cached body, refreshed expiry
            if (response.StatusCode == HttpStatusCode.NotModified && cached is not null)
            {
                _logger.LogDebug("ESI cache hit (304) for {Url}", url);
                Cache(url, cached with { Expires = response.Content.Headers.Expires });
                return JsonSerializer.Deserialize<T>(cached.Body, JsonOptions);
            }

            if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                await DelayForRetryOrThrowAsync(response, url, attempt, ct);
                continue;
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return default;
            }

            await EnsureSuccessAsync(response, url, ct);

            var body = await response.Content.ReadAsStringAsync(ct);
            var etag = response.Headers.ETag?.Tag;
            var expiry = response.Content.Headers.Expires;
            if (etag is not null || expiry is not null)
            {
                Cache(url, new CachedResponse(etag, body, expiry));
            }

            return JsonSerializer.Deserialize<T>(body, JsonOptions);
        }
    }

    /// <summary>
    /// Perform a POST request to the ESI API. Returns null only for 404.
    /// </summary>
    public async Task<T?> PostAsync<T>(string path, object? content = null, CancellationToken ct = default)
    {
        var url = AppendDatasource(path);

        for (var attempt = 0; ; attempt++)
        {
            await WaitIfErrorLimitExhaustedAsync(ct);

            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (content is not null)
            {
                request.Content = JsonContent.Create(content);
            }

            using var response = await SendAsync(request, url, ct);

            TrackErrorLimits(response);
            TrackRateLimits(response);

            if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                await DelayForRetryOrThrowAsync(response, url, attempt, ct);
                continue;
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return default;
            }

            await EnsureSuccessAsync(response, url, ct);

            return await response.Content.ReadFromJsonAsync<T>(JsonOptions, ct);
        }
    }

    private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, string url, CancellationToken ct)
    {
        try
        {
            return await _httpClient.SendAsync(request, ct);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request failed for {Url}", url);
            throw new EsiApiException(0, $"network error while calling ESI: {ex.Message}");
        }
        catch (TaskCanceledException) when (!ct.IsCancellationRequested)
        {
            _logger.LogError("HTTP request timed out after {Timeout}s for {Url}", _httpClient.Timeout.TotalSeconds, url);
            throw new EsiApiException(0, $"ESI request timed out after {_httpClient.Timeout.TotalSeconds:0}s.");
        }
    }

    private async Task DelayForRetryOrThrowAsync(HttpResponseMessage response, string url, int attempt, CancellationToken ct)
    {
        if (attempt >= MaxRetries)
        {
            _logger.LogError("Rate limited on {Url}; giving up after {Attempts} attempts", url, attempt + 1);
            throw new EsiApiException(429, "ESI rate limit exceeded; retry later.");
        }

        var retryAfter = response.Headers.RetryAfter?.Delta ?? TimeSpan.FromSeconds(10);
        if (retryAfter > MaxRetryDelay)
        {
            retryAfter = MaxRetryDelay;
        }
        _logger.LogWarning("Rate limited on {Url}. Retrying after {Seconds}s", url, retryAfter.TotalSeconds);
        await Task.Delay(retryAfter, ct);
    }

    private async Task EnsureSuccessAsync(HttpResponseMessage response, string url, CancellationToken ct)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var status = (int)response.StatusCode;
        if (status >= 500)
        {
            _logger.LogError("ESI server error {StatusCode} for {Url}", status, url);
            throw new EsiApiException(status, $"ESI server error {status} — the EVE API is having issues, retry later.");
        }

        var detail = await ReadErrorDetailAsync(response, ct);
        _logger.LogError("ESI client error {StatusCode} for {Url}: {Detail}", status, url, detail);
        throw new EsiApiException(status, $"ESI rejected the request ({status} {response.StatusCode}): {detail}");
    }

    private static async Task<string> ReadErrorDetailAsync(HttpResponseMessage response, CancellationToken ct)
    {
        try
        {
            var body = await response.Content.ReadAsStringAsync(ct);
            return body.Length > 200 ? body[..200] : body;
        }
        catch
        {
            return "(no error detail)";
        }
    }

    private void Cache(string url, CachedResponse entry)
    {
        _cache.Set(url, entry, new MemoryCacheEntryOptions
        {
            // Size accounting keeps the cache under MaxCacheSizeBytes
            Size = entry.Body.Length * sizeof(char) + url.Length * sizeof(char) + 64,
            SlidingExpiration = CacheSlidingExpiration
        });
    }

    private async Task WaitIfErrorLimitExhaustedAsync(CancellationToken ct)
    {
        TimeSpan delay;
        lock (_errorLimitLock)
        {
            if (_errorLimitRemaining > 5 || DateTimeOffset.UtcNow >= _errorLimitReset)
            {
                return;
            }
            delay = _errorLimitReset - DateTimeOffset.UtcNow;
        }

        _logger.LogWarning("ESI error limit nearly exhausted. Waiting {Delay}s before next request.", delay.TotalSeconds);
        await Task.Delay(delay, ct);
    }

    private void TrackErrorLimits(HttpResponseMessage response)
    {
        int? remain = null;
        int? resetSeconds = null;

        if (response.Headers.TryGetValues("X-ESI-Error-Limit-Remain", out var remainValues)
            && int.TryParse(remainValues.FirstOrDefault(), out var r))
        {
            remain = r;
        }
        if (response.Headers.TryGetValues("X-ESI-Error-Limit-Reset", out var resetValues)
            && int.TryParse(resetValues.FirstOrDefault(), out var s))
        {
            resetSeconds = s;
        }

        if (remain is null && resetSeconds is null)
        {
            return;
        }

        lock (_errorLimitLock)
        {
            if (remain is not null)
                _errorLimitRemaining = remain.Value;
            if (resetSeconds is not null)
                _errorLimitReset = DateTimeOffset.UtcNow.AddSeconds(resetSeconds.Value);
        }
    }

    private void TrackRateLimits(HttpResponseMessage response)
    {
        if (response.Headers.TryGetValues("X-Ratelimit-Remaining", out var remainValues)
            && int.TryParse(remainValues.FirstOrDefault(), out var remaining) && remaining < 10)
        {
            _logger.LogWarning("ESI rate limit low: {Remaining} tokens remaining for group {Group}",
                remaining,
                response.Headers.TryGetValues("X-Ratelimit-Group", out var groupValues)
                    ? groupValues.FirstOrDefault()
                    : "unknown");
        }
    }

    private string AppendDatasource(string path)
    {
        var separator = path.Contains('?') ? '&' : '?';
        return $"{path}{separator}datasource={_options.Datasource}";
    }

    public void Dispose() => _cache.Dispose();
}
