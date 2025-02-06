using InventoryService.Business.Abstraction; // Importa l'interfaccia della logica di business
using InventoryService.Business; // Importa l'implementazione della logica di business
using InventoryService.Repository; // Importa il livello repository
using InventoryService.Repository.Abstraction; // Importa le interfacce dei repository
using InventoryService.ClientHttp; // Importa il client HTTP per le comunicazioni con altri servizi
using InventoryService.ClientHttp.Abstraction; // Importa l'interfaccia del client HTTP
// using InventoryService.Kafka; // Importa la gestione di Kafka per la messaggistica
// using InventoryService.Kafka.Abstraction; // Importa l'interfaccia per Kafka
using Microsoft.EntityFrameworkCore; // Importa il supporto per Entity Framework Core (Database ORM)

// • I servizi REST in ASP.NET Core posso essere implementati basandosi sui
// Controller di MVC oppure nel formato Minimal APIs.

// Noi usere il controller di MVC

var builder = WebApplication.CreateBuilder(args); // Crea il builder dell'applicazione

// Configura il server Kestrel per ascoltare sulla porta 5001
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000); // Il server accetta connessioni su qualsiasi IP sulla porta 5001
});

// Configura il DbContext per collegarsi a SQL Server usando la stringa di connessione dal file di configurazione
builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("InventoryDb")));

// Registra il repository dei prodotti nel container DI (Dependency Injection)
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Registra il repository della coda transazionale per Kafka
// builder.Services.AddScoped<ITransactionalOutboxRepository, TransactionalOutboxRepository>();

// Registra il livello di business per la gestione dell'inventario
builder.Services.AddScoped<IInventoryBusiness, InventoryBusiness>();

// Configura un client HTTP per la comunicazione con il servizio degli ordini
builder.Services.AddHttpClient<IClientHttp, ClientHttp>(client =>
{
    client.BaseAddress = new Uri("http://orderservice:5000"); // Indirizzo del servizio degli ordini
});

// Configura Kafka Producer per l'invio di messaggi
// builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>(sp =>
//     new KafkaProducer("kafka:9092")); // Configura Kafka sulla porta 9092

// Configura Kafka Consumer per l'ascolto dei messaggi
// builder.Services.AddSingleton<IKafkaConsumer, KafkaConsumer>(sp =>
//     new KafkaConsumer("kafka:9092", "inventory-service-group")); // Assegna il consumer a un gruppo

// Aggiunge il supporto ai controller API
builder.Services.AddControllers();

// Configura Swagger per la documentazione delle API
builder.Services.AddEndpointsApiExplorer(); // Aggiunge il supporto alle Minimal APIs, anche se noi utilizzeremo i controller
builder.Services.AddSwaggerGen();

var app = builder.Build(); // Costruisce l'applicazione

// Configura Swagger solo se l'app è in modalità sviluppo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Abilita la redirezione HTTPS per garantire connessioni sicure
app.UseHttpsRedirection(); // -> Aggiunge il middleware per reindirizzare le richieste HTTP verso HTTPS

// Abilita l'autorizzazione (in futuro potrebbe includere autenticazione)
app.UseAuthorization(); // -> Aggiunge il middleware per le funzionalità di autorizzazione

app.MapControllers(); // Mappa i controller alle route HTTP
                     // Aggiunge gli endpoint per le action dei controller: permettono di utilizzare le
                    // funzionalità di routing necessarie a inoltrare le richieste alle action.
 
app.Run(); // Avvia l'applicazione