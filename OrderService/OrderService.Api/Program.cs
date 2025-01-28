using OrderService.Business;
using OrderService.Business.Abstraction;
using OrderService.ClientHttp;
using OrderService.ClientHttp.Abstraction;
using OrderService.Repository;
using OrderService.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;
using OrderService.Kafka;
using OrderService.Kafka.Abstraction;

var builder = WebApplication.CreateBuilder(args);

// Aggiungi il contesto Db al contenitore di servizi
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OrderDb")));

// Registra i repository
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddScoped<ITransactionalOutboxRepository, TransactionalOutboxRepository>();

// Registra i servizi business
builder.Services.AddScoped<IOrderBusiness, OrderBusiness>();

// Configura HttpClient
builder.Services.AddHttpClient<IClientHttp, ClientHttp>(client =>
{
    client.BaseAddress = new Uri("http://inventoryservice:5001");
});

// Configura Kafka
builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>(sp =>
    new KafkaProducer("kafka:9092", sp.GetRequiredService<ITransactionalOutboxRepository>()));

builder.Services.AddSingleton<IKafkaConsumer, KafkaConsumer>(sp =>
    new KafkaConsumer("kafka:9092", "order-service-group", sp.GetRequiredService<IProductRepository>()));


// Configura Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Creazione e configurazione dell'applicazione
var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

// Configura Swagger per la documentazione API
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
