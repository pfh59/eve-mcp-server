using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace eve_mcp_server.Tests;

/// <summary>
/// A delegating handler that returns pre-configured responses for testing.
/// </summary>
public sealed class MockHttpMessageHandler : DelegatingHandler
{
    private readonly Queue<HttpResponseMessage> _responses = new();
    private readonly List<HttpRequestMessage> _requests = new();

    public IReadOnlyList<HttpRequestMessage> Requests => _requests;
    public HttpRequestMessage LastRequest => _requests[^1];

    /// <summary>Queue a response to be returned on the next SendAsync call.</summary>
    public MockHttpMessageHandler QueueResponse(HttpStatusCode statusCode, object? body = null, string? etag = null, TimeSpan? expiresIn = null)
    {
        var response = new HttpResponseMessage(statusCode);
        if (body is not null)
        {
            response.Content = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json");
            if (expiresIn is not null)
            {
                response.Content.Headers.Expires = DateTimeOffset.UtcNow.Add(expiresIn.Value);
            }
        }
        if (etag is not null)
        {
            response.Headers.ETag = new EntityTagHeaderValue($"\"{etag}\"");
        }
        // Default ESI headers
        response.Headers.TryAddWithoutValidation("X-ESI-Error-Limit-Remain", "100");
        response.Headers.TryAddWithoutValidation("X-ESI-Error-Limit-Reset", "60");

        _responses.Enqueue(response);
        return this;
    }

    /// <summary>Queue a 200 OK response with a JSON body.</summary>
    public MockHttpMessageHandler QueueJsonResponse<T>(T body, string? etag = null, TimeSpan? expiresIn = null)
        => QueueResponse(HttpStatusCode.OK, body, etag, expiresIn);

    /// <summary>Queue a 404 Not Found response.</summary>
    public MockHttpMessageHandler Queue404()
        => QueueResponse(HttpStatusCode.NotFound);

    /// <summary>Queue a single 429 Too Many Requests response.</summary>
    public MockHttpMessageHandler Queue429()
    {
        var rateLimited = new HttpResponseMessage(HttpStatusCode.TooManyRequests);
        rateLimited.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromMilliseconds(10));
        rateLimited.Headers.TryAddWithoutValidation("X-ESI-Error-Limit-Remain", "100");
        rateLimited.Headers.TryAddWithoutValidation("X-ESI-Error-Limit-Reset", "60");
        _responses.Enqueue(rateLimited);
        return this;
    }

    /// <summary>Queue a 304 Not Modified response.</summary>
    public MockHttpMessageHandler Queue304()
    {
        var response = new HttpResponseMessage(HttpStatusCode.NotModified);
        response.Headers.TryAddWithoutValidation("X-ESI-Error-Limit-Remain", "100");
        response.Headers.TryAddWithoutValidation("X-ESI-Error-Limit-Reset", "60");
        _responses.Enqueue(response);
        return this;
    }

    /// <summary>Queue a 429 Too Many Requests response followed by a success response.</summary>
    public MockHttpMessageHandler Queue429ThenSuccess<T>(T body)
    {
        var rateLimited = new HttpResponseMessage(HttpStatusCode.TooManyRequests);
        rateLimited.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromMilliseconds(50));
        rateLimited.Headers.TryAddWithoutValidation("X-ESI-Error-Limit-Remain", "100");
        rateLimited.Headers.TryAddWithoutValidation("X-ESI-Error-Limit-Reset", "60");
        _responses.Enqueue(rateLimited);
        QueueJsonResponse(body);
        return this;
    }

    /// <summary>Queue a 500 Internal Server Error response.</summary>
    public MockHttpMessageHandler Queue500()
    {
        var error = new HttpResponseMessage(HttpStatusCode.InternalServerError);
        error.Headers.TryAddWithoutValidation("X-ESI-Error-Limit-Remain", "95");
        error.Headers.TryAddWithoutValidation("X-ESI-Error-Limit-Reset", "60");
        _responses.Enqueue(error);
        return this;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _requests.Add(request);
        if (_responses.Count == 0)
            throw new InvalidOperationException($"No queued response for {request.Method} {request.RequestUri}");
        return Task.FromResult(_responses.Dequeue());
    }
}
