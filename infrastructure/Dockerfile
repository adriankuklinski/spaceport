FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

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
ENTRYPOINT ["dotnet", "Spaceport.Host.dll"]