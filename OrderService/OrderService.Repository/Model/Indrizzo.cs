using System;

namespace OrderService.Repository.Model
{
    // Rappresenta un indirizzo associato a un cliente
    // Questa classe è usata in OrderDbContext.cs per definire la tabella Indirizzi.
    // È collegata alla tabella Clienti tramite una relazione uno-a-molti.
    public class Indirizzo
    {
        public int Id { get; set; } // Identificativo univoco dell'indirizzo
        public int ClienteId { get; set; } // Chiave esterna che collega l'indirizzo al cliente
        public string? Tipo { get; set; } // Tipo di indirizzo: "Spedizione", "Fatturazione", ecc.
        public string? IndirizzoCompleto { get; set; } // Indirizzo dettagliato
        public string? NumeroCivico { get; set; } // Numero civico dell'indirizzo
        public string? Cap { get; set; } // Codice di avviamento postale (CAP)
        public string? Provincia { get; set; } // Provincia di residenza
        public string? Localita { get; set; } // Città o località dell'indirizzo

        public Cliente? Cliente { get; set; } // Proprietà di navigazione verso il cliente associato
    }
}
