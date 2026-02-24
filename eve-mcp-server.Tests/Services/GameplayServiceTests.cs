using eve_mcp_server.Models;
using eve_mcp_server.Services;

namespace eve_mcp_server.Tests.Services;

public class GameplayServiceTests
{
    private readonly GameplayService _svc;
    private readonly MockHttpMessageHandler _handler;

    public GameplayServiceTests()
    {
        var (client, handler) = TestEsiClientFactory.Create();
        _handler = handler;
        _svc = new GameplayService(client);
    }

    [Fact]
    public async Task GetStatusAsync_ReturnsServerStatus()
    {
        var status = new
        {
            players = 25000,
            server_version = "2134567",
            start_time = "2024-01-01T11:00:00Z"
        };
        _handler.QueueJsonResponse(status);

        var result = await _svc.GetStatusAsync();

        Assert.NotNull(result);
        Assert.Equal(25000, result.Players);
    }

    [Fact]
    public async Task GetKillmailAsync_ReturnsKillmailDetails()
    {
        var killmail = new
        {
            killmail_id = 12345,
            killmail_time = "2024-01-01T12:00:00Z",
            solar_system_id = 30000142,
            victim = new { ship_type_id = 587, character_id = 111L }
        };
        _handler.QueueJsonResponse(killmail);

        var result = await _svc.GetKillmailAsync(12345, "abcdef");

        Assert.NotNull(result);
        Assert.Equal(12345, result.KillmailId);
    }

    [Fact]
    public async Task GetWarsAsync_ReturnsWarIds()
    {
        _handler.QueueJsonResponse(new[] { 1000L, 999L, 998L });

        var result = await _svc.GetWarsAsync();

        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetWarsAsync_WithMaxWarId_IncludesParameter()
    {
        _handler.QueueJsonResponse(new[] { 500L, 499L });

        await _svc.GetWarsAsync(maxWarId: 501);

        Assert.Contains("max_war_id=501", _handler.LastRequest.RequestUri!.ToString());
    }

    [Fact]
    public async Task GetWarAsync_ReturnsWarDetails()
    {
        var war = new
        {
            id = 1000,
            declared = "2024-01-01T00:00:00Z",
            mutual = false,
            aggressor = new { corporation_id = 98000001 },
            defender = new { corporation_id = 98000002 }
        };
        _handler.QueueJsonResponse(war);

        var result = await _svc.GetWarAsync(1000);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetIncursionsAsync_ReturnsIncursions()
    {
        var incursions = new[]
        {
            new { constellation_id = 20000001, state = "established", influence = 0.5, staging_solar_system_id = 30000001 }
        };
        _handler.QueueJsonResponse(incursions);

        var result = await _svc.GetIncursionsAsync();

        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetInsurancePricesAsync_ReturnsPrices()
    {
        var prices = new[]
        {
            new { type_id = 587, levels = new[] { new { cost = 1000.0, payout = 5000.0, name = "Basic" } } }
        };
        _handler.QueueJsonResponse(prices);

        var result = await _svc.GetInsurancePricesAsync();

        Assert.NotNull(result);
        Assert.Single(result);
    }
}
