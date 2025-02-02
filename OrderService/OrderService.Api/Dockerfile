# Usare l'immagine base .NET per runtime
FROM mcr.microsoft.com/dotnet/nightly/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5000

# Usare l'immagine per build
FROM mcr.microsoft.com/dotnet/nightly/sdk:9.0 AS build
WORKDIR /src
COPY ["OrderService.Api/OrderService.Api.csproj", "OrderService.Api/"]
COPY ["OrderService.Business/OrderService.Business.csproj", "OrderService.Business/"]
COPY ["OrderService.Repository/OrderService.Repository.csproj", "OrderService.Repository/"]
COPY ["OrderService.Shared/OrderService.Shared.csproj", "OrderService.Shared/"]
COPY ["OrderService.ClientHttp/OrderService.ClientHttp.csproj", "OrderService.ClientHttp/"]
RUN dotnet restore "OrderService.Api/OrderService.Api.csproj"
COPY . .
WORKDIR "/src/OrderService.Api"
RUN dotnet build "OrderService.Api.csproj" -c Release -o /app/build

# Pubblicare l'app
FROM build AS publish
RUN dotnet publish "OrderService.Api.csproj" -c Release -o /app/publish

# Eseguire l'app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderService.Api.dll"]
