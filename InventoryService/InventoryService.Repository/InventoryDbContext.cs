using InventoryService.Repository.Model;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Repository
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity =>
            {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Stock).IsRequired();
            entity.Property(e => e.Price).HasPrecision(18, 2);
            });

            modelBuilder.Entity<TransactionalOutbox>(entity =>
            {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Message).IsRequired();
            entity.Property(e => e.Table).IsRequired().HasMaxLength(50);
            });
        }
    }
}
