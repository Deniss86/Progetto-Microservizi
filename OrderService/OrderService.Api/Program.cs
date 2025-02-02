using OrderService.Business; // Importa la logica di business
using OrderService.Business.Abstraction; // Importa l'interfaccia della logica di business
using OrderService.ClientHttp; // Importa il client HTTP per comunicare con altri servizi
using OrderService.ClientHttp.Abstraction; // Importa l'interfaccia del client HTTP
using OrderService.Repository; // Importa il livello repository
using OrderService.Repository.Abstraction; // Importa le interfacce dei repository
using Microsoft.EntityFrameworkCore; // Importa il supporto per Entity Framework Core (Database ORM)
//using OrderService.Kafka; // Importa la gestione di Kafka
//using OrderService.Kafka.Abstraction; // Importa l'interfaccia per Kafka
/*
Configura il database con SQL Server e Entity Framework Core.
Registra i servizi per la gestione degli ordini, repository e outbox transazionale.
Configura Kafka Producer e Consumer per la comunicazione tra servizi.
Aggiunge il client HTTP per connettersi al servizio InventoryService.
Configura Swagger per generare la documentazione API.
*/

var builder = WebApplication.CreateBuilder(args); // Crea il builder dell'applicazione

// Configura il server Kestrel per ascoltare sulla porta 5001
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000); // Il server accetta connessioni su qualsiasi IP sulla porta 5001
});

// Configura il contesto del database e connessione a SQL Server
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OrderDb"),
        b => b.MigrationsAssembly("OrderService.Api")));

// Registra il repository degli ordini nel container DI (Dependency Injection)
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Registra il repository dell'outbox transazionale per Kafka
//builder.Services.AddScoped<ITransactionalOutboxRepository, TransactionalOutboxRepository>();

// Registra il livello di business per la gestione degli ordini
builder.Services.AddScoped<IOrderBusiness, OrderBusiness>();

// Configura un client HTTP per la comunicazione con il servizio di inventario
builder.Services.AddHttpClient<IClientHttp, ClientHttp>(client =>
{
    client.BaseAddress = new Uri("http://inventoryservice:5001"); // Indirizzo del servizio di inventario
});

// Configura Kafka Producer per l'invio di messaggi Kafka
/*
builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>(sp =>
    new KafkaProducer("kafka:9092", sp.GetRequiredService<ITransactionalOutboxRepository>()));
*/

// Configura Kafka Consumer per l'ascolto dei messaggi Kafka
/*
builder.Services.AddSingleton<IKafkaConsumer, KafkaConsumer>(sp =>
    new KafkaConsumer("kafka:9092", "order-service-group", sp.GetRequiredService<IProductRepository>()));
*/

// Configura Swagger per la documentazione delle API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Aggiunge il supporto per l'autorizzazione
builder.Services.AddAuthorization();
builder.Services.AddControllers();

// Creazione e configurazione dell'applicazione
var app = builder.Build();

app.UseHttpsRedirection(); // Abilita la redirezione HTTPS per garantire connessioni sicure
app.UseAuthorization(); // Abilita l'autorizzazione

app.MapControllers(); // Mappa i controller alle route HTTP

// Configura Swagger per la documentazione API solo in ambiente di sviluppo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run(); // Avvia l'applicazione