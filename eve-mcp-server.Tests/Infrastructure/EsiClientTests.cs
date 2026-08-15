using System.Net;
using eve_mcp_server.Infrastructure;

namespace eve_mcp_server.Tests.Infrastructure;

public class EsiClientTests
{
    [Fact]
    public async Task GetAsync_ReturnsDeserializedResponse()
    {
        var (client, handler) = TestEsiClientFactory.Create();
        handler.QueueJsonResponse(new[] { 10000001L, 10000002L });

        var result = await client.GetAsync<List<long>>("/universe/regions/");

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(10000002L, result);
    }

    [Fact]
    public async Task GetAsync_SendsUserAgentHeader()
    {
        var (client, handler) = TestEsiClientFactory.Create();
        handler.QueueJsonResponse(new[] { 1L });

        await client.GetAsync<List<long>>("/universe/regions/");

        var ua = handler.LastRequest.Headers.UserAgent.ToString();
        Assert.Contains("eve-mcp-server-tests", ua);
    }

    [Fact]
    public async Task GetAsync_SendsCompatibilityDateHeader()
    {
        var (client, handler) = TestEsiClientFactory.Create();
        handler.QueueJsonResponse(new[] { 1L });

        await client.GetAsync<List<long>>("/universe/regions/");

        Assert.True(handler.LastRequest.Headers.TryGetValues("X-Compatibility-Date", out var values));
        Assert.Matches(@"^\d{4}-\d{2}-\d{2}$", values!.Single());
    }

    [Fact]
    public async Task GetAsync_AppendsDatasource()
    {
        var (client, handler) = TestEsiClientFactory.Create();
        handler.QueueJsonResponse(new[] { 1L });

        await client.GetAsync<List<long>>("/universe/regions/");

        Assert.Contains("datasource=tranquility", handler.LastRequest.RequestUri!.Query);
    }

    [Fact]
    public async Task GetAsync_AppendsDatasource_WithExistingQueryString()
    {
        var (client, handler) = TestEsiClientFactory.Create();
        handler.QueueJsonResponse(new[] { 1L });

        await client.GetAsync<List<long>>("/markets/10000002/orders/?order_type=all");

        var query = handler.LastRequest.RequestUri!.Query;
        Assert.Contains("order_type=all", query);
        Assert.Contains("datasource=tranquility", query);
    }

    [Fact]
    public async Task GetAsync_CachesETagAndSendsIfNoneMatch()
    {
        var (client, handler) = TestEsiClientFactory.Create();

        // First call: returns 200 with ETag
        handler.QueueJsonResponse(new[] { 1L, 2L }, etag: "abc123");
        var first = await client.GetAsync<List<long>>("/universe/regions/");

        // Second call: returns 304 → should use cached value
        handler.Queue304();
        var second = await client.GetAsync<List<long>>("/universe/regions/");

        Assert.Equal(first, second);

        // Verify If-None-Match was sent on second request
        var secondRequest = handler.Requests[1];
        Assert.Contains("\"abc123\"", secondRequest.Headers.IfNoneMatch.ToString());
    }

    [Fact]
    public async Task GetAsync_HonorsExpires_ServesFromCacheWithoutRequest()
    {
        var (client, handler) = TestEsiClientFactory.Create();
        handler.QueueJsonResponse(new[] { 1L, 2L }, etag: "abc123", expiresIn: TimeSpan.FromMinutes(5));

        var first = await client.GetAsync<List<long>>("/universe/regions/");
        // No second response queued: a network call would throw in the mock handler
        var second = await client.GetAsync<List<long>>("/universe/regions/");

        Assert.Equal(first, second);
        Assert.Single(handler.Requests);
    }

    [Fact]
    public async Task GetAsync_RetriesOn429WithRetryAfter()
    {
        var (client, handler) = TestEsiClientFactory.Create();
        handler.Queue429ThenSuccess(new[] { 42L });

        var result = await client.GetAsync<List<long>>("/test/");

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(42L, result[0]);
        Assert.Equal(2, handler.Requests.Count); // 429 + retry
    }

    [Fact]
    public async Task GetAsync_GivesUpAfterRepeated429()
    {
        var (client, handler) = TestEsiClientFactory.Create();
        handler.Queue429().Queue429().Queue429();

        var ex = await Assert.ThrowsAsync<EsiApiException>(() => client.GetAsync<List<long>>("/test/"));

        Assert.Equal(429, ex.StatusCode);
        Assert.Equal(3, handler.Requests.Count); // initial + 2 retries, no infinite loop
    }

    [Fact]
    public async Task GetAsync_ReturnsNullOn404()
    {
        var (client, handler) = TestEsiClientFactory.Create();
        handler.Queue404();

        var result = await client.GetAsync<List<long>>("/test/");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAsync_ThrowsOn5xx()
    {
        var (client, handler) = TestEsiClientFactory.Create();
        handler.Queue500();

        var ex = await Assert.ThrowsAsync<EsiApiException>(() => client.GetAsync<List<long>>("/test/"));

        Assert.Equal(500, ex.StatusCode);
    }

    [Fact]
    public async Task GetAsync_ThrowsOn4xxOtherThan404()
    {
        var (client, handler) = TestEsiClientFactory.Create();
        handler.QueueResponse(HttpStatusCode.BadRequest, new { error = "invalid parameter" });

        var ex = await Assert.ThrowsAsync<EsiApiException>(() => client.GetAsync<List<long>>("/test/"));

        Assert.Equal(400, ex.StatusCode);
        Assert.Contains("invalid parameter", ex.Message);
    }

    [Fact]
    public async Task PostAsync_SendsJsonBodyAndReturnsResult()
    {
        var (client, handler) = TestEsiClientFactory.Create();
        var expected = new { characters = new[] { new { id = 123L, name = "Test" } } };
        handler.QueueJsonResponse(expected);

        var result = await client.PostAsync<object>("/universe/ids/", new[] { "Test" });

        Assert.NotNull(result);
        Assert.Equal(HttpMethod.Post, handler.LastRequest.Method);
    }

    [Fact]
    public async Task PostAsync_RetriesOn429()
    {
        var (client, handler) = TestEsiClientFactory.Create();
        handler.Queue429ThenSuccess(new[] { 1L, 2L });

        var result = await client.PostAsync<List<long>>("/test/", new[] { "a" });

        Assert.NotNull(result);
        Assert.Equal(2, handler.Requests.Count);
    }

    [Fact]
    public async Task PostAsync_GivesUpAfterRepeated429()
    {
        var (client, handler) = TestEsiClientFactory.Create();
        handler.Queue429().Queue429().Queue429();

        var ex = await Assert.ThrowsAsync<EsiApiException>(() => client.PostAsync<List<long>>("/test/", new[] { "a" }));

        Assert.Equal(429, ex.StatusCode);
        Assert.Equal(3, handler.Requests.Count);
    }
}
