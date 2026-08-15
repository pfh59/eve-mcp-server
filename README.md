# EVE MCP Server

[![CI](https://github.com/pfh59/eve-mcp-server/actions/workflows/ci.yml/badge.svg)](https://github.com/pfh59/eve-mcp-server/actions/workflows/ci.yml)

A [Model Context Protocol (MCP)](https://modelcontextprotocol.io/) server for [EVE Online's ESI API](https://esi.evetech.net/ui/), built in C# with .NET 10.

This server exposes **52 MCP tools** covering the public ESI endpoints, enabling AI assistants (Claude, Copilot, etc.) to query EVE Online data — regions, market orders, killmails, sovereignty, industry, and more.

## Features

- **52 MCP tools** across 6 domains, all prefixed `eve_` (e.g. `eve_get_market_orders`)
- **ESI best practices**: identifying User-Agent, `X-Compatibility-Date` versioning, Expires + ETag caching (If-None-Match/304), error limit throttling, bounded 429 retry with Retry-After
- **Clean architecture**: Infrastructure → Models → Services → Tools
- **Stdio transport** for seamless integration with MCP-compatible clients

## Tool Domains

| Domain | Tools | Examples |
|--------|-------|---------|
| **Universe** | 21 | Regions, constellations, solar systems, stars, stations, stargates, planets, moons, types, groups, categories, factions, races, bloodlines, ancestries, system jumps/kills |
| **Market** | 6 | Prices, orders by region/type, history, market groups, types in region |
| **Character** | 6 | Character/corporation/alliance public info, affiliations |
| **Search** | 2 | Name → ID resolution, ID → name resolution |
| **Gameplay** | 7 | Server status, killmails, wars, war killmails, incursions, insurance |
| **Infrastructure** | 10 | Routes, industry facilities/systems, sovereignty map/structures, dogma attributes/effects, loyalty store offers |

## Project Structure

```
eve-mcp-server/
├── Infrastructure/
│   ├── EsiClient.cs           # Central HTTP client (caching, rate limits, retries)
│   ├── EsiClientOptions.cs    # ESI configuration (base URL, User-Agent, datasource, ...)
│   └── EsiApiException.cs     # Typed error surfaced to tools for 4xx/5xx/rate limits
├── Models/
│   ├── UniverseModels.cs      # Region, Constellation, SolarSystem, EveType, etc.
│   ├── MarketModels.cs        # MarketOrder, MarketPrice, MarketHistory, etc.
│   ├── CharacterModels.cs     # CharacterPublicInfo, CorporationPublicInfo, etc.
│   ├── SearchModels.cs        # UniverseIdsResult, UniverseName
│   ├── GameplayModels.cs      # ServerStatus, Killmail, War, Incursion
│   └── InfrastructureModels.cs # IndustryFacility, SovereigntyMap, DogmaAttribute, etc.
├── Services/
│   ├── UniverseService.cs
│   ├── MarketService.cs
│   ├── CharacterService.cs
│   ├── SearchService.cs
│   ├── GameplayService.cs
│   └── InfrastructureService.cs
├── Tools/
│   ├── ToolRunner.cs          # Shared serialization / error-mapping wrapper
│   ├── UniverseTools.cs
│   ├── MarketTools.cs
│   ├── CharacterTools.cs
│   ├── SearchTools.cs
│   ├── GameplayTools.cs
│   └── InfrastructureTools.cs
└── Program.cs                 # Host setup, DI, MCP server bootstrap
```

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later

## Build & Run

```bash
dotnet build
dotnet run --project eve-mcp-server
```

## Testing

```bash
dotnet test
```

## Configuration

All ESI client settings can be overridden through the `ESI` configuration section
(environment variables use the `ESI__` prefix):

| Environment variable | Default | Purpose |
|----------------------|---------|---------|
| `ESI__UserAgent` | `eve-mcp-server/1.0.0 (+https://github.com/pfh59/eve-mcp-server)` | Identifies your app to CCP. **Set this to include your own contact email** — CCP throttles unidentifiable agents. |
| `ESI__BaseUrl` | `https://esi.evetech.net` | ESI base URL (unversioned routes). |
| `ESI__Datasource` | `tranquility` | `tranquility` or `singularity`. |
| `ESI__CompatibilityDate` | `2026-08-15` | [ESI compatibility date](https://developers.eveonline.com/blog/changing-versions-v42-was-getting-out-of-hand) sent as `X-Compatibility-Date`. |
| `ESI__TimeoutSeconds` | `30` | Per-request HTTP timeout. |
| `ESI__MaxCacheSizeBytes` | `33554432` | Upper bound of the in-memory response cache. |

Example for Claude Desktop:

```json
{
  "mcpServers": {
    "eve-online": {
      "command": "dotnet",
      "args": ["run", "--project", "/path/to/eve-mcp-server/eve-mcp-server"],
      "env": {
        "ESI__UserAgent": "my-eve-assistant/1.0 (you@example.com; +https://github.com/you/your-fork)"
      }
    }
  }
}
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
      "command": "/path/to/eve-mcp-server/eve-mcp-server/bin/Debug/net10.0/eve-mcp-server"
    }
  }
}
```

## ESI Best Practices

This server follows [EVE ESI best practices](https://developers.eveonline.com/docs/services/esi/best-practices/):

- **User-Agent**: Every request identifies the application (configurable, see above)
- **Versioning**: Unversioned routes with the `X-Compatibility-Date` header (the legacy `/latest` routes are being removed by CCP)
- **Expires caching**: A resource is never re-requested before its `Expires` timestamp — CCP can ban applications that bypass ESI caching
- **ETag caching**: Responses are cached with ETags; revalidation uses `If-None-Match` to get cheap 304 responses. The cache is size-bounded (LRU)
- **Error limit**: Tracks `X-ESI-Error-Limit-Remain` (shared across all tools) and pauses requests when near the limit
- **Rate limiting**: Monitors `X-Ratelimit-Remaining` per bucket group and logs warnings when running low
- **429 handling**: Retries after `Retry-After` (capped at 60s), at most twice, then reports the failure
- **Error semantics**: 404 → "not found"; 5xx and other 4xx are reported to the model as explicit ESI errors, never as "not found"

## License

MIT
