using Microsoft.EntityFrameworkCore;
using OrderService.Repository;

var builder = WebApplication.CreateBuilder(args);

// Aggiungi il contesto Db al contenitore di servizi
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OrderDb")));

// Aggiungi i controller
builder.Services.AddControllers();

var app = builder.Build();

// Configura il middleware
app.UseRouting();
app.MapControllers();
app.Run();
