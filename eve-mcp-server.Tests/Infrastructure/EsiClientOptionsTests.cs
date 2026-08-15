using eve_mcp_server.Infrastructure;

namespace eve_mcp_server.Tests.Infrastructure;

public class EsiClientOptionsTests
{
    [Fact]
    public void DefaultValues_AreCorrect()
    {
        var options = new EsiClientOptions();

        Assert.Equal("https://esi.evetech.net", options.BaseUrl);
        Assert.Contains("eve-mcp-server", options.UserAgent);
        Assert.Equal("tranquility", options.Datasource);
        Assert.Matches(@"^\d{4}-\d{2}-\d{2}$", options.CompatibilityDate);
        Assert.InRange(options.TimeoutSeconds, 1, 100);
        Assert.True(options.MaxCacheSizeBytes > 0);
    }

    [Fact]
    public void Properties_CanBeOverridden()
    {
        var options = new EsiClientOptions
        {
            BaseUrl = "https://custom.url",
            UserAgent = "custom-agent/2.0",
            Datasource = "singularity",
            CompatibilityDate = "2026-01-01",
            TimeoutSeconds = 10,
            MaxCacheSizeBytes = 1024
        };

        Assert.Equal("https://custom.url", options.BaseUrl);
        Assert.Equal("custom-agent/2.0", options.UserAgent);
        Assert.Equal("singularity", options.Datasource);
        Assert.Equal("2026-01-01", options.CompatibilityDate);
        Assert.Equal(10, options.TimeoutSeconds);
        Assert.Equal(1024, options.MaxCacheSizeBytes);
    }
}
