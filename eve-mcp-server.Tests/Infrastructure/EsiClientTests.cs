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
    public async Task GetAsync_ReturnsDefaultOn5xx()
    {
        var (client, handler) = TestEsiClientFactory.Create();
        handler.Queue500();

        var result = await client.GetAsync<List<long>>("/test/");

        Assert.Null(result);
    }

    [Fact]
    public async Task PostAsync_SendsJsonBodyAndReturnsResult()
    {
        var (client, handler) = TestEsiClientFactory.Create();
        var expected = new { characters = new[] { new { id = 123L, name = "Test" } } };
        handler.QueueJsonResponse(expected);

        var result = await client.PostAsync<object>("/universe/ids/", new[] { "Test" });

        Assert.NotNull(result);
        // Verify POST method was used
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
    public async Task GetAllPagesAsync_FetchesMultiplePages()
    {
        var (client, handler) = TestEsiClientFactory.Create();
        handler.QueueJsonResponse(new[] { 1L, 2L });
        handler.QueueJsonResponse(new[] { 3L });
        handler.QueueJsonResponse(Array.Empty<long>()); // Empty page signals end

        var result = await client.GetAllPagesAsync<long>("/test/");

        Assert.Equal(3, result.Count);
        Assert.Equal(new[] { 1L, 2L, 3L }, result);
    }
}
