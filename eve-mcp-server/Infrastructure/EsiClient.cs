using System.Collections.Concurrent;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace eve_mcp_server.Infrastructure;

/// <summary>
/// Central HTTP client for ESI following best practices:
/// - User-Agent header on every request
/// - ETag caching with If-None-Match
/// - Rate limit tracking (X-Ratelimit headers)
/// - Error limit tracking (X-ESI-Error-Limit headers)
/// - Proper retry logic for 429/5xx
/// </summary>
public sealed class EsiClient
{
    private readonly HttpClient _httpClient;
    private readonly EsiClientOptions _options;
    private readonly ILogger<EsiClient> _logger;

    // ETag cache: URL → (ETag, cached JSON string)
    private readonly ConcurrentDictionary<string, (string ETag, string Body)> _etagCache = new();

    // Rate limit tracking
    private int _errorLimitRemaining = 100;
    private DateTimeOffset _errorLimitReset = DateTimeOffset.MinValue;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public EsiClient(HttpClient httpClient, EsiClientOptions options, ILogger<EsiClient> logger)
    {
        _httpClient = httpClient;
        _options = options;
        _logger = logger;

        _httpClient.BaseAddress = new Uri(_options.BaseUrl);
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(_options.UserAgent);
    }

    /// <summary>
    /// Perform a GET request to the ESI API with caching and rate limit awareness.
    /// </summary>
    public async Task<T?> GetAsync<T>(string path, CancellationToken ct = default)
    {
        var url = AppendDatasource(path);

        // Check error limit before making a request
        if (_errorLimitRemaining <= 5 && DateTimeOffset.UtcNow < _errorLimitReset)
        {
            var delay = _errorLimitReset - DateTimeOffset.UtcNow;
            _logger.LogWarning("ESI error limit nearly exhausted ({Remaining} left). Waiting {Delay}s before next request.",
                _errorLimitRemaining, delay.TotalSeconds);
            await Task.Delay(delay, ct);
        }

        using var request = new HttpRequestMessage(HttpMethod.Get, url);

        // Add ETag for conditional request (saves bandwidth & rate limit tokens)
        if (_etagCache.TryGetValue(url, out var cached))
        {
            request.Headers.IfNoneMatch.ParseAdd(cached.ETag);
        }

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.SendAsync(request, ct);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request failed for {Url}", url);
            throw;
        }

        // Track error limits from response headers
        TrackErrorLimits(response);
        TrackRateLimits(response);

        // 304 Not Modified → return cached result (costs only 1 token)
        if (response.StatusCode == HttpStatusCode.NotModified && cached.Body is not null)
        {
            _logger.LogDebug("ESI cache hit (304) for {Url}", url);
            return JsonSerializer.Deserialize<T>(cached.Body, JsonOptions);
        }

        // 429 Too Many Requests → wait and retry once
        if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            var retryAfter = response.Headers.RetryAfter?.Delta ?? TimeSpan.FromSeconds(10);
            _logger.LogWarning("Rate limited on {Url}. Retrying after {Seconds}s", url, retryAfter.TotalSeconds);
            await Task.Delay(retryAfter, ct);
            return await GetAsync<T>(path, ct);
        }

        // 5xx → log and return default (0 tokens penalty)
        if ((int)response.StatusCode >= 500)
        {
            _logger.LogError("ESI server error {StatusCode} for {Url}", (int)response.StatusCode, url);
            return default;
        }

        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadAsStringAsync(ct);

        // Cache the ETag for future conditional requests
        var etag = response.Headers.ETag?.Tag;
        if (etag is not null)
        {
            _etagCache[url] = (etag, body);
        }

        return JsonSerializer.Deserialize<T>(body, JsonOptions);
    }

    /// <summary>
    /// Perform a POST request to the ESI API.
    /// </summary>
    public async Task<T?> PostAsync<T>(string path, object? content = null, CancellationToken ct = default)
    {
        var url = AppendDatasource(path);

        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        if (content is not null)
        {
            request.Content = JsonContent.Create(content);
        }

        var response = await _httpClient.SendAsync(request, ct);

        TrackErrorLimits(response);
        TrackRateLimits(response);

        if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            var retryAfter = response.Headers.RetryAfter?.Delta ?? TimeSpan.FromSeconds(10);
            _logger.LogWarning("Rate limited on POST {Url}. Retrying after {Seconds}s", url, retryAfter.TotalSeconds);
            await Task.Delay(retryAfter, ct);
            return await PostAsync<T>(path, content, ct);
        }

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<T>(JsonOptions, ct);
    }

    /// <summary>
    /// Fetch all pages of a paginated resource.
    /// </summary>
    public async Task<List<T>> GetAllPagesAsync<T>(string path, CancellationToken ct = default)
    {
        var results = new List<T>();
        var separator = path.Contains('?') ? '&' : '?';
        var page = 1;

        while (true)
        {
            var pagedUrl = $"{path}{separator}page={page}";
            var items = await GetAsync<List<T>>(pagedUrl, ct);

            if (items is null || items.Count == 0)
                break;

            results.AddRange(items);
            page++;

            // Safety: ESI rarely has more than 100 pages
            if (page > 200) break;
        }

        return results;
    }

    private string AppendDatasource(string path)
    {
        var separator = path.Contains('?') ? '&' : '?';
        return $"{path}{separator}datasource={_options.Datasource}";
    }

    private void TrackErrorLimits(HttpResponseMessage response)
    {
        if (response.Headers.TryGetValues("X-ESI-Error-Limit-Remain", out var remainValues))
        {
            if (int.TryParse(remainValues.FirstOrDefault(), out var remain))
                _errorLimitRemaining = remain;
        }

        if (response.Headers.TryGetValues("X-ESI-Error-Limit-Reset", out var resetValues))
        {
            if (int.TryParse(resetValues.FirstOrDefault(), out var resetSeconds))
                _errorLimitReset = DateTimeOffset.UtcNow.AddSeconds(resetSeconds);
        }
    }

    private void TrackRateLimits(HttpResponseMessage response)
    {
        if (response.Headers.TryGetValues("X-Ratelimit-Remaining", out var remainValues))
        {
            if (int.TryParse(remainValues.FirstOrDefault(), out var remaining) && remaining < 10)
            {
                _logger.LogWarning("ESI rate limit low: {Remaining} tokens remaining for group {Group}",
                    remaining,
                    response.Headers.TryGetValues("X-Ratelimit-Group", out var groupValues) 
                        ? groupValues.FirstOrDefault() 
                        : "unknown");
            }
        }
    }
}
