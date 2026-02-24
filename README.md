# EVE MCP Server

A [Model Context Protocol (MCP)](https://modelcontextprotocol.io/) server for [EVE Online's ESI API](https://esi.evetech.net/ui/), built in C# with .NET 8.

This server exposes **52 MCP tools** covering the public ESI endpoints, enabling AI assistants (Claude, Copilot, etc.) to query EVE Online data — regions, market orders, killmails, sovereignty, industry, and more.

## Features

- **52 MCP tools** across 6 domains
- **ESI best practices**: User-Agent header, ETag caching (If-None-Match/304), rate limit tracking, error limit throttling, 429 retry with Retry-After, 5xx graceful degradation
- **Clean architecture**: Infrastructure → Models → Services → Tools
- **Stdio transport** for seamless integration with MCP-compatible clients

## Tool Domains

| Domain | Tools | Examples |
|--------|-------|---------|
| **Universe** | 20 | Regions, constellations, solar systems, stars, stations, stargates, planets, moons, types, groups, categories, factions, races, bloodlines, ancestries, system jumps/kills |
| **Market** | 6 | Prices, orders by region/type, history, market groups, types in region |
| **Character** | 6 | Character/corporation/alliance public info, affiliations |
| **Search** | 2 | Name → ID resolution, ID → name resolution |
| **Gameplay** | 8 | Server status, killmails, wars, war killmails, incursions, insurance |
| **Infrastructure** | 10 | Routes, industry facilities/systems, sovereignty map/structures, dogma attributes/effects, loyalty store offers |

## Project Structure

```
eve-mcp-server/
├── Infrastructure/
│   ├── EsiClient.cs           # Central HTTP client (caching, rate limits, retries)
│   └── EsiClientOptions.cs    # ESI configuration (base URL, User-Agent, datasource)
├── Models/
│   ├── UniverseModels.cs      # Region, Constellation, SolarSystem, EveType, etc.
│   ├── MarketModels.cs        # MarketOrder, MarketPrice, MarketHistory, etc.
│   ├── CharacterModels.cs     # CharacterPublicInfo, CorporationPublicInfo, etc.
│   ├── SearchModels.cs        # UniverseIdsResult, UniverseName
│   ├── GameplayModels.cs      # ServerStatus, Killmail, War, Incursion
│   └── InfrastructureModels.cs # RouteResult, IndustryFacility, SovereigntyMap, etc.
├── Services/
│   ├── UniverseService.cs
│   ├── MarketService.cs
│   ├── CharacterService.cs
│   ├── SearchService.cs
│   ├── GameplayService.cs
│   └── InfrastructureService.cs
├── Tools/
│   ├── UniverseTools.cs
│   ├── MarketTools.cs
│   ├── CharacterTools.cs
│   ├── SearchTools.cs
│   ├── GameplayTools.cs
│   └── InfrastructureTools.cs
└── Program.cs                 # Host setup, DI, MCP server bootstrap
```

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later

## Build & Run

```bash
dotnet build
dotnet run --project eve-mcp-server
```

## Testing

```bash
dotnet test
```

## MCP Client Configuration

### Claude Desktop / VS Code

Add to your MCP settings (`claude_desktop_config.json` or VS Code MCP settings):

```json
{
  "mcpServers": {
    "eve-online": {
      "command": "dotnet",
      "args": ["run", "--project", "/path/to/eve-mcp-server/eve-mcp-server"]
    }
  }
}
```

### Using the compiled binary

```json
{
  "mcpServers": {
    "eve-online": {
      "command": "/path/to/eve-mcp-server/eve-mcp-server/bin/Debug/net8.0/eve-mcp-server"
    }
  }
}
```

## ESI Best Practices

This server follows [EVE ESI best practices](https://docs.esi.evetech.net/docs/best_practices):

- **User-Agent**: Every request includes `eve-mcp-server/1.0.0` to identify the application
- **ETag caching**: Responses are cached with ETags; subsequent requests use `If-None-Match` to get 304 responses (1 token cost instead of 2)
- **Error limit**: Tracks `X-ESI-Error-Limit-Remain` and pauses requests when near the 100-error/minute limit
- **Rate limiting**: Monitors `X-Ratelimit-Remaining` per bucket group and logs warnings when running low
- **429 handling**: Automatically retries after `Retry-After` delay
- **5xx handling**: Logs errors and returns gracefully (no error-limit penalty)

## License

MIT
