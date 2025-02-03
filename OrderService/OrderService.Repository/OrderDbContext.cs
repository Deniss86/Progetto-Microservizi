using Microsoft.EntityFrameworkCore; // Importa Entity Framework Core
using OrderService.Repository.Model; // Importa i modelli del repository

namespace OrderService.Repository
{
    // Contesto del database per Entity Framework
    /*
    Definisce le tabelle:
    Orders → Per la gestione degli ordini.
    Clienti e Indirizzi → Per informazioni sui clienti e la fatturazione.
    TransactionalOutbox → Per i messaggi Kafka non ancora elaborati (commentato).
    */
    public class OrderDbContext : DbContext
    {
        // Costruttore che accetta le opzioni di configurazione
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

        // Definizione delle tabelle del database
        public DbSet<Order> Orders { get; set; }
        public DbSet<Cliente> Clienti { get; set; }
        public DbSet<Indirizzo> Indirizzi { get; set; }
        //public DbSet<TransactionalOutbox> TransactionalOutbox { get; set; } // Commentato per rimuovere Kafka

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurazione della tabella Order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.TotalPrice).HasPrecision(18, 2);
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
                entity.Property(e => e.Tipo).IsRequired().HasMaxLength(20);
            });

            // Configurazione della tabella TransactionalOutbox (commentata per rimuovere Kafka)
            /*
            modelBuilder.Entity<TransactionalOutbox>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Tabella).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Messaggio).IsRequired();
            });
            */
        }
    }
}