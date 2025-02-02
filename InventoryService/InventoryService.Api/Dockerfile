# Usare l'immagine base .NET per runtime
FROM mcr.microsoft.com/dotnet/nightly/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5001

# Usare l'immagine per build
FROM mcr.microsoft.com/dotnet/nightly/sdk:9.0 AS build
WORKDIR /src
COPY ["InventoryService.Api/InventoryService.Api.csproj", "InventoryService.Api/"]
COPY ["InventoryService.Business/InventoryService.Business.csproj", "InventoryService.Business/"]
COPY ["InventoryService.Repository/InventoryService.Repository.csproj", "InventoryService.Repository/"]
COPY ["InventoryService.Shared/InventoryService.Shared.csproj", "InventoryService.Shared/"]
COPY ["InventoryService.ClientHttp/InventoryService.ClientHttp.csproj", "InventoryService.ClientHttp/"]
#COPY ["InventoryService.Kafka/InventoryService.Kafka.csproj", "InventoryService.Kafka/"]
RUN dotnet restore "InventoryService.Api/InventoryService.Api.csproj"
COPY . .
WORKDIR "/src/InventoryService.Api"
RUN dotnet build "InventoryService.Api.csproj" -c Release -o /app/build

# Pubblicare l'app
FROM build AS publish
RUN dotnet publish "InventoryService.Api.csproj" -c Release -o /app/publish

# Eseguire l'app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InventoryService.Api.dll"]
