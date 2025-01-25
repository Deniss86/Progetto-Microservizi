# Usa l'immagine base di .NET
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000

# Usa l'immagine base di .NET SDK per buildare il progetto
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copia i file di progetto e ripristina le dipendenze
COPY ["OrderService.Api/OrderService.Api.csproj", "OrderService.Api/"]
COPY ["OrderService.Business/OrderService.Business.csproj", "OrderService.Business/"]
COPY ["OrderService.ClientHttp/OrderService.ClientHttp.csproj", "OrderService.ClientHttp/"]
COPY ["OrderService.Kafka/OrderService.Kafka.csproj", "OrderService.Kafka/"]
COPY ["OrderService.Repository/OrderService.Repository.csproj", "OrderService.Repository/"]
COPY ["OrderService.Shared/OrderService.Shared.csproj", "OrderService.Shared/"]
RUN dotnet restore "OrderService.Api/OrderService.Api.csproj"

# Copia tutto il contenuto del progetto e builda
COPY . .
WORKDIR "/src/OrderService.Api"
RUN dotnet build "OrderService.Api.csproj" -c Release -o /app/build

# Pubblica l'applicazione
FROM build AS publish
RUN dotnet publish "OrderService.Api.csproj" -c Release -o /app/publish

# Configura il runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderService.Api.dll"]
