using InventoryService.Repository.Model; // Importa i modelli
using Microsoft.EntityFrameworkCore; // Importa Entity Framework Core

//InventoryDbContext è il contesto Entity Framework per il database.
namespace InventoryService.Repository
{
    // Contesto del database per Entity Framework
    public class InventoryDbContext : DbContext
    {
        // Costruttore che accetta le opzioni di configurazione
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }

        // Definisce la tabella dei prodotti
        public DbSet<Product> Products { get; set; }

        // Configura il modello del database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configura la tabella Product
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id); // Chiave primaria
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Stock).IsRequired();
                entity.Property(e => e.Price).HasPrecision(18, 2); // Precisione decimale
            });
        }
    }
}