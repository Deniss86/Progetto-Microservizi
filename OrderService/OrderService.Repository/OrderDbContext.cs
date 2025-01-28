using Microsoft.EntityFrameworkCore;
using OrderService.Shared.Models;
using OrderService.Repository.Model;

namespace OrderService.Repository
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

        // DbSet per le entità esistenti
        public DbSet<Order> Orders { get; set; }

        // Aggiungi i DbSet per Cliente e Indirizzo
        public DbSet<Cliente> Clienti { get; set; }
        public DbSet<Indirizzo> Indirizzi { get; set; }

        public DbSet<TransactionalOutbox> TransactionalOutbox { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurazione della tabella Order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.TotalPrice)
                    .HasPrecision(18, 2);
            });

            // Configurazione della tabella Cliente
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Cognome).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CodiceFiscale).HasMaxLength(16);
                entity.HasMany(c => c.Indirizzi)
                      .WithOne(i => i.Cliente)
                      .HasForeignKey(i => i.ClienteId);
            });

            // Configurazione della tabella Indirizzo
            modelBuilder.Entity<Indirizzo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.IndirizzoCompleto).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Tipo).IsRequired().HasMaxLength(20); // "Spedizione" o "Fatturazione"
                entity.Property(e => e.Cap).HasMaxLength(5);
                entity.Property(e => e.Provincia).HasMaxLength(2);
                entity.Property(e => e.Localita).HasMaxLength(100);
            });

            // Configurazione della tabella TransactionalOutbox
            modelBuilder.Entity<TransactionalOutbox>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Tabella).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Messaggio).IsRequired();
            });
        }
    }
}
