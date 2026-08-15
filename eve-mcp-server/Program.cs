using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using eve_mcp_server.Infrastructure;
using eve_mcp_server.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddConsole(consoleLogOptions =>
{
    // Configure all logs to go to stderr
    consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
});

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

// Infrastructure — overridable via appsettings.json or ESI__* environment variables
var esiOptions = builder.Configuration.GetSection("ESI").Get<EsiClientOptions>() ?? new EsiClientOptions();
builder.Services.AddSingleton(esiOptions);
builder.Services
    .AddHttpClient(nameof(EsiClient), http => http.Timeout = TimeSpan.FromSeconds(esiOptions.TimeoutSeconds))
    .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
    {
        // The client below is a singleton, so rotate pooled connections to pick up DNS changes
        PooledConnectionLifetime = TimeSpan.FromMinutes(15)
    });

// One shared EsiClient so the response cache and ESI error-limit state see all traffic
builder.Services.AddSingleton(sp => new EsiClient(
    sp.GetRequiredService<IHttpClientFactory>().CreateClient(nameof(EsiClient)),
    sp.GetRequiredService<EsiClientOptions>(),
    sp.GetRequiredService<ILoggerFactory>().CreateLogger<EsiClient>()));

// Services
builder.Services.AddSingleton<UniverseService>();
builder.Services.AddSingleton<MarketService>();
builder.Services.AddSingleton<SearchService>();
builder.Services.AddSingleton<CharacterService>();
builder.Services.AddSingleton<GameplayService>();
builder.Services.AddSingleton<InfrastructureService>();

await builder.Build().RunAsync();
