using Microsoft.EntityFrameworkCore;
using OrderService.Repository;
using OrderService.Repository.Abstraction;
using OrderService.Business;
using OrderService.Business.Abstraction;

var builder = WebApplication.CreateBuilder(args);

// Aggiungi il contesto Db al contenitore di servizi
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OrderDb")));

// Registra il repository
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Registra il livello Business
builder.Services.AddScoped<IOrderBusiness, OrderBusiness>();

// Aggiungi i controller
builder.Services.AddControllers();

// Configura Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Order Service API",
        Version = "v1",
        Description = "API per la gestione degli ordini in un sistema di e-commerce.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "prova",
            Email = "prova@esempio.com"
        }
    });
});

var app = builder.Build();

// Configura il middleware
app.UseRouting();

// Abilita Swagger in ambiente di sviluppo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
