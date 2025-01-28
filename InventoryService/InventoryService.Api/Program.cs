using InventoryService.Business.Abstraction;
using InventoryService.Business;
using InventoryService.Repository;
using InventoryService.Repository.Abstraction;
using InventoryService.ClientHttp;
using InventoryService.ClientHttp.Abstraction;
using InventoryService.Kafka;
using InventoryService.Kafka.Abstraction;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5001);
});
// Configura DbContext
builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("InventoryDb")));

// Registra i repository
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<ITransactionalOutboxRepository, TransactionalOutboxRepository>();

// Registra il livello Business
builder.Services.AddScoped<IInventoryBusiness, InventoryBusiness>();

// Configura ClientHttp
builder.Services.AddHttpClient<IClientHttp, ClientHttp>(client =>
{
    client.BaseAddress = new Uri("http://orderservice:5000");
});

// Configura Kafka
builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>(sp =>
    new KafkaProducer("kafka:9092"));

builder.Services.AddSingleton<IKafkaConsumer, KafkaConsumer>(sp =>
    new KafkaConsumer("kafka:9092", "inventory-service-group"));
// Aggiungi i controller
builder.Services.AddControllers();

// Configura Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configura Swagger solo in modalità sviluppo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
