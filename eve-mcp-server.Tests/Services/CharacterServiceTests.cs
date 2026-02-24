using eve_mcp_server.Models;
using eve_mcp_server.Services;

namespace eve_mcp_server.Tests.Services;

public class CharacterServiceTests
{
    private readonly CharacterService _svc;
    private readonly MockHttpMessageHandler _handler;

    public CharacterServiceTests()
    {
        var (client, handler) = TestEsiClientFactory.Create();
        _handler = handler;
        _svc = new CharacterService(client);
    }

    [Fact]
    public async Task GetCharacterAsync_ReturnsCharacterInfo()
    {
        var character = new
        {
            name = "CCP Bartender",
            birthday = "2010-03-15T00:00:00Z",
            corporation_id = 98000001,
            description = "A CCP dev"
        };
        _handler.QueueJsonResponse(character);

        var result = await _svc.GetCharacterAsync(12345);

        Assert.NotNull(result);
        Assert.Equal("CCP Bartender", result.Name);
    }

    [Fact]
    public async Task GetCorporationAsync_ReturnsCorporationInfo()
    {
        var corp = new
        {
            name = "C C P",
            ticker = "CCP",
            member_count = 50,
            ceo_id = 12345L
        };
        _handler.QueueJsonResponse(corp);

        var result = await _svc.GetCorporationAsync(98000001);

        Assert.NotNull(result);
        Assert.Equal("C C P", result.Name);
        Assert.Equal("CCP", result.Ticker);
    }

    [Fact]
    public async Task GetAlliancesAsync_ReturnsAllianceIds()
    {
        _handler.QueueJsonResponse(new[] { 99000001L, 99000002L });

        var result = await _svc.GetAlliancesAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetAllianceAsync_ReturnsAllianceInfo()
    {
        var alliance = new
        {
            name = "Goonswarm Federation",
            ticker = "CONDI",
            date_founded = "2010-06-01T00:00:00Z",
            executor_corporation_id = 98000001
        };
        _handler.QueueJsonResponse(alliance);

        var result = await _svc.GetAllianceAsync(99000001);

        Assert.NotNull(result);
        Assert.Equal("Goonswarm Federation", result.Name);
    }

    [Fact]
    public async Task GetAffiliationsAsync_PostsCharacterIdsAndReturnsResult()
    {
        var affiliations = new[]
        {
            new { character_id = 123L, corporation_id = 456L, alliance_id = 789L }
        };
        _handler.QueueJsonResponse(affiliations);

        var result = await _svc.GetAffiliationsAsync(new List<long> { 123 });

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(HttpMethod.Post, _handler.LastRequest.Method);
    }

    [Fact]
    public async Task GetAllianceCorporationsAsync_ReturnsCorporationIds()
    {
        _handler.QueueJsonResponse(new[] { 98000001L, 98000002L, 98000003L });

        var result = await _svc.GetAllianceCorporationsAsync(99000001);

        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
    }
}
