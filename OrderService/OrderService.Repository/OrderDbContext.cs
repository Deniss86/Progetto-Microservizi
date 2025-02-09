using Microsoft.EntityFrameworkCore; // // Importa Entity Framework Core per la gestione del database
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
        public DbSet<Order> Orders { get; set; } // Tabella per gli ordini
        public DbSet<Cliente> Clienti { get; set; } // Tabella per i clienti
        public DbSet<Indirizzo> Indirizzi { get; set; } // Tabella per gli indirizzi
        //public DbSet<TransactionalOutbox> TransactionalOutbox { get; set; } // Commentato per rimuovere Kafka

        // Configurazione delle tabelle del database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurazione della tabella Order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id); // Imposta la chiave primaria
                entity.Property(e => e.Status).IsRequired(); // Campo obbligatorio
                entity.Property(e => e.TotalPrice).HasPrecision(18, 2); // Precisione per i valori monetari
            });

            // Configurazione della tabella Cliente
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.Id); // Chiave primaria
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(50); // Campo obbligatorio, max 50 caratteri
                entity.Property(e => e.Cognome).IsRequired().HasMaxLength(50); // Campo obbligatorio, max 50 caratteri
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100); // Campo obbligatorio, max 100 caratteri
                entity.Property(e => e.CodiceFiscale).HasMaxLength(16); // Codice fiscale (non obbligatorio)
                entity.HasMany(c => c.Indirizzi) // Relazione 1-N con Indirizzo
                      .WithOne(i => i.Cliente)
                      .HasForeignKey(i => i.ClienteId);
            });

            // Configurazione della tabella Indirizzo
            modelBuilder.Entity<Indirizzo>(entity =>
            {
                entity.HasKey(e => e.Id); // Chiave primaria
                entity.Property(e => e.IndirizzoCompleto).IsRequired().HasMaxLength(200); // Campo obbligatorio, max 200 caratteri
                entity.Property(e => e.Tipo).IsRequired().HasMaxLength(20); // Tipo di indirizzo (es. Residenza, Fatturazione)
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