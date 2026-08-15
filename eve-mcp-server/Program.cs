using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using eve_mcp_server.Esi;
using eve_mcp_server.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddConsole(consoleLogOptions =>
{
    // stdout carries the JSON-RPC stream; all logs must go to stderr
    consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
});

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

// Overridable via appsettings.json or ESI__* environment variables
var esiOptions = builder.Configuration.GetSection("ESI").Get<EsiClientOptions>() ?? new EsiClientOptions();
builder.Services.AddSingleton(esiOptions);
builder.Services
    .AddHttpClient(nameof(EsiClient), http => http.Timeout = TimeSpan.FromSeconds(esiOptions.TimeoutSeconds))
    .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
    {
        // EsiClient is a singleton: rotate pooled connections so DNS changes are picked up
        PooledConnectionLifetime = TimeSpan.FromMinutes(15)
    });

// One shared EsiClient so the response cache and ESI error-limit state see all traffic
builder.Services.AddSingleton(sp => new EsiClient(
    sp.GetRequiredService<IHttpClientFactory>().CreateClient(nameof(EsiClient)),
    sp.GetRequiredService<EsiClientOptions>(),
    sp.GetRequiredService<ILoggerFactory>().CreateLogger<EsiClient>()));

builder.Services.AddSingleton<UniverseService>();
builder.Services.AddSingleton<MarketService>();
builder.Services.AddSingleton<SearchService>();
builder.Services.AddSingleton<CharacterService>();
builder.Services.AddSingleton<GameplayService>();
builder.Services.AddSingleton<InfrastructureService>();

await builder.Build().RunAsync();
