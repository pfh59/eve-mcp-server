using eve_mcp_server.Infrastructure;

namespace eve_mcp_server.Tests.Infrastructure;

public class EsiClientOptionsTests
{
    [Fact]
    public void DefaultValues_AreCorrect()
    {
        var options = new EsiClientOptions();

        Assert.Equal("https://esi.evetech.net/latest", options.BaseUrl);
        Assert.Contains("eve-mcp-server", options.UserAgent);
        Assert.Equal("tranquility", options.Datasource);
    }

    [Fact]
    public void Properties_CanBeOverridden()
    {
        var options = new EsiClientOptions
        {
            BaseUrl = "https://custom.url",
            UserAgent = "custom-agent/2.0",
            Datasource = "singularity"
        };

        Assert.Equal("https://custom.url", options.BaseUrl);
        Assert.Equal("custom-agent/2.0", options.UserAgent);
        Assert.Equal("singularity", options.Datasource);
    }
}
