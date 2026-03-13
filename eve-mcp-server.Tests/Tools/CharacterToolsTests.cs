using eve_mcp_server.Services;
using eve_mcp_server.Tools;

namespace eve_mcp_server.Tests.Tools;

public class CharacterToolsTests
{
    private readonly CharacterService _svc;
    private readonly MockHttpMessageHandler _handler;

    public CharacterToolsTests()
    {
        var (client, handler) = TestEsiClientFactory.Create();
        _handler = handler;
        _svc = new CharacterService(client);
    }

    [Fact]
    public async Task GetCharacter_ReturnsJson()
    {
        _handler.QueueJsonResponse(new { name = "Test Pilot", birthday = "2010-01-01T00:00:00Z", corporation_id = 98000001 });

        var result = await CharacterTools.GetCharacter(_svc, 12345, CancellationToken.None);

        Assert.Contains("Test Pilot", result);
        Assert.Contains("98000001", result);
    }

    [Fact]
    public async Task GetCharacter_WhenNotFound_ReturnsMessage()
    {
        _handler.Queue500();

        var result = await CharacterTools.GetCharacter(_svc, 999, CancellationToken.None);

        Assert.Equal("Character not found.", result);
    }

    [Fact]
    public async Task GetCharacterAffiliations_ReturnsJson()
    {
        _handler.QueueJsonResponse(new[]
        {
            new { character_id = 12345, corporation_id = 98000001, alliance_id = 99000001 }
        });

        var result = await CharacterTools.GetCharacterAffiliations(_svc, "12345", CancellationToken.None);

        Assert.Contains("12345", result);
        Assert.Contains("98000001", result);
    }

    [Fact]
    public async Task GetCharacterAffiliations_InvalidId_ReturnsErrorMessage()
    {
        var result = await CharacterTools.GetCharacterAffiliations(_svc, "12345,abc,67890", CancellationToken.None);

        Assert.Contains("Invalid character ID: 'abc'", result);
    }

    [Fact]
    public async Task GetCorporation_ReturnsJson()
    {
        _handler.QueueJsonResponse(new { name = "Test Corp", ticker = "TCOR", member_count = 42 });

        var result = await CharacterTools.GetCorporation(_svc, 98000001, CancellationToken.None);

        Assert.Contains("Test Corp", result);
        Assert.Contains("42", result);
    }

    [Fact]
    public async Task GetCorporation_WhenNotFound_ReturnsMessage()
    {
        _handler.Queue500();

        var result = await CharacterTools.GetCorporation(_svc, 999, CancellationToken.None);

        Assert.Equal("Corporation not found.", result);
    }

    [Fact]
    public async Task GetAlliances_ReturnsAllianceIds()
    {
        _handler.QueueJsonResponse(new[] { 99000001L, 99000002L });

        var result = await CharacterTools.GetAlliances(_svc, CancellationToken.None);

        Assert.Contains("99000001", result);
    }

    [Fact]
    public async Task GetAlliance_ReturnsAllianceJson()
    {
        _handler.QueueJsonResponse(new { name = "Test Alliance", ticker = "TALI" });

        var result = await CharacterTools.GetAlliance(_svc, 99000001, CancellationToken.None);

        Assert.Contains("Test Alliance", result);
    }

    [Fact]
    public async Task GetAlliance_WhenNotFound_ReturnsMessage()
    {
        _handler.Queue500();

        var result = await CharacterTools.GetAlliance(_svc, 999, CancellationToken.None);

        Assert.Equal("Alliance not found.", result);
    }

    [Fact]
    public async Task GetAllianceCorporations_ReturnsCorporationIds()
    {
        _handler.QueueJsonResponse(new[] { 98000001L, 98000002L });

        var result = await CharacterTools.GetAllianceCorporations(_svc, 99000001, CancellationToken.None);

        Assert.Contains("98000001", result);
    }
}
