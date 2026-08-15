using eve_mcp_server.Services;
using eve_mcp_server.Tools;

namespace eve_mcp_server.Tests.Tools;

public class GameplayToolsTests
{
    private readonly GameplayService _svc;
    private readonly MockHttpMessageHandler _handler;

    public GameplayToolsTests()
    {
        var (client, handler) = TestEsiClientFactory.Create();
        _handler = handler;
        _svc = new GameplayService(client);
    }

    [Fact]
    public async Task GetServerStatus_ReturnsJson()
    {
        _handler.QueueJsonResponse(new { players = 25000, server_version = "2134567" });

        var result = await GameplayTools.GetServerStatus(_svc, CancellationToken.None);

        Assert.Contains("25000", result);
    }

    [Fact]
    public async Task GetServerStatus_WhenEsiIsDown_ReturnsEsiErrorMessage()
    {
        _handler.Queue500();

        var result = await GameplayTools.GetServerStatus(_svc, CancellationToken.None);

        Assert.StartsWith("ESI request failed", result);
        Assert.Contains("500", result);
    }

    [Fact]
    public async Task GetKillmail_ReturnsKillmailJson()
    {
        _handler.QueueJsonResponse(new
        {
            killmail_id = 12345,
            killmail_time = "2024-01-01T12:00:00Z",
            solar_system_id = 30000142
        });

        var result = await GameplayTools.GetKillmail(_svc, 12345, ValidHash, CancellationToken.None);

        Assert.Contains("12345", result);
        Assert.Contains("30000142", result);
    }

    private const string ValidHash = "a1b2c3d4e5f6a7b8c9d0a1b2c3d4e5f6a7b8c9d0";

    [Fact]
    public async Task GetKillmail_WhenNotFound_ReturnsMessage()
    {
        _handler.Queue404();

        var result = await GameplayTools.GetKillmail(_svc, 999, ValidHash, CancellationToken.None);

        Assert.Equal("Killmail not found.", result);
    }

    [Fact]
    public async Task GetKillmail_InvalidHash_IsRejectedWithoutHttpCall()
    {
        var result = await GameplayTools.GetKillmail(_svc, 999, "../status/?x=", CancellationToken.None);

        Assert.StartsWith("Invalid parameter", result);
        Assert.Empty(_handler.Requests);
    }

    [Fact]
    public async Task GetWars_ReturnsWarIds()
    {
        _handler.QueueJsonResponse(new[] { 1000L, 999L });

        var result = await GameplayTools.GetWars(_svc, null, CancellationToken.None);

        Assert.Contains("1000", result);
    }

    [Fact]
    public async Task GetIncursions_ReturnsIncursionList()
    {
        _handler.QueueJsonResponse(new[]
        {
            new { constellation_id = 20000001, state = "established" }
        });

        var result = await GameplayTools.GetIncursions(_svc, CancellationToken.None);

        Assert.Contains("established", result);
    }
}
