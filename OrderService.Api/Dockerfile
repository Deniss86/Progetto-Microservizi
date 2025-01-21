# Usa l'immagine base nightly SDK per .NET 9.0
FROM mcr.microsoft.com/dotnet/nightly/sdk:9.0 AS build
WORKDIR /src

# Copia tutti i file di progetto
COPY ["OrderService.Api/OrderService.Api.csproj", "OrderService.Api/"]
COPY ["OrderService.Business/OrderService.Business.csproj", "OrderService.Business/"]
COPY ["OrderService.Shared/OrderService.Shared.csproj", "OrderService.Shared/"]
COPY ["OrderService.Repository/OrderService.Repository.csproj", "OrderService.Repository/"]

# Ripristina i pacchetti NuGet
RUN dotnet restore "OrderService.Api/OrderService.Api.csproj"

# Copia tutto il contenuto del progetto
COPY . .

# Compila l'applicazione
WORKDIR "/src/OrderService.Api"
RUN dotnet build -c Release -o /app/build

# Pubblica l'applicazione
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Usa l'immagine runtime nightly per eseguire l'app
FROM mcr.microsoft.com/dotnet/nightly/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderService.Api.dll"]
