FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/Spaceport.API/Spaceport.API.csproj", "src/Spaceport.API/"]
COPY ["src/Spaceport.Domain/Spaceport.Domain.csproj", "src/Spaceport.Domain/"]
COPY ["src/Spaceport.Infrastructure/Spaceport.Infrastructure.csproj", "src/Spaceport.Infrastructure/"]
COPY ["src/Spaceport.Host/Spaceport.Host.csproj", "src/Spaceport.Host/"]
RUN dotnet restore "src/Spaceport.Host/Spaceport.Host.csproj"
COPY . .
WORKDIR "/src/src/Spaceport.Host"
RUN dotnet build "Spaceport.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Spaceport.Host.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create non-root user for security
RUN addgroup --system --gid 1000 appgroup && \
    adduser --system --uid 1000 --ingroup appgroup --shell /bin/sh appuser && \
    chown -R appuser:appgroup /app
USER appuser

# Health check
HEALTHCHECK --interval=30s --timeout=3s --retries=3 CMD wget -qO- http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "Spaceport.Host.dll"]