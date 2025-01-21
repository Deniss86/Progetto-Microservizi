using Microsoft.EntityFrameworkCore;
using OrderService.Shared.Models;

namespace OrderService.Repository
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.TotalPrice)
                    .HasPrecision(18, 2);
            });
        }
    }
}
