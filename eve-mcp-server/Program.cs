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

// Infrastructure
builder.Services.AddSingleton<EsiClientOptions>();
builder.Services.AddHttpClient<EsiClient>();

// Services
builder.Services.AddSingleton<UniverseService>();
builder.Services.AddSingleton<MarketService>();
builder.Services.AddSingleton<SearchService>();
builder.Services.AddSingleton<CharacterService>();
builder.Services.AddSingleton<GameplayService>();
builder.Services.AddSingleton<InfrastructureService>();

await builder.Build().RunAsync();