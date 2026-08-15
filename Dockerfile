# Build on the host architecture, cross-compile for the target one (no QEMU needed)
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG TARGETARCH
WORKDIR /src

COPY eve-mcp-server/eve-mcp-server.csproj eve-mcp-server/
RUN dotnet restore eve-mcp-server/eve-mcp-server.csproj -a $TARGETARCH

COPY eve-mcp-server/ eve-mcp-server/
RUN dotnet publish eve-mcp-server/eve-mcp-server.csproj -c Release -a $TARGETARCH --no-restore -o /app

# Chiseled runtime: distroless, runs as non-root by default
FROM mcr.microsoft.com/dotnet/runtime:10.0-noble-chiseled
LABEL org.opencontainers.image.source="https://github.com/pfh59/eve-mcp-server" \
      org.opencontainers.image.description="MCP server exposing EVE Online's public ESI API" \
      org.opencontainers.image.licenses="MIT"
WORKDIR /app
COPY --from=build /app .

# MCP stdio transport: run with `docker run -i` so stdin stays open
ENTRYPOINT ["dotnet", "eve-mcp-server.dll"]
