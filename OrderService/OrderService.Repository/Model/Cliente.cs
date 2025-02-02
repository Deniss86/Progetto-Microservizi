using System;
using System.Collections.Generic;
// Questa classe è usata in OrderDbContext.cs per definire la tabella Clienti.
// È collegata alla tabella Indirizzi tramite una relazione uno-a-molti.
namespace OrderService.Repository.Model
{
    // Rappresenta un cliente nel sistema
    public class Cliente
    {
        public int Id { get; set; } // Identificativo univoco del cliente
        public string? Nome { get; set; } // Nome del cliente
        public string? Cognome { get; set; } // Cognome del cliente
        public string? Email { get; set; } // Indirizzo email del cliente
        public string? CodiceFiscale { get; set; } // Codice fiscale del cliente (opzionale)
        public DateTime DataDiNascita { get; set; } // Data di nascita del cliente

        public List<Indirizzo> Indirizzi { get; set; } = new List<Indirizzo>(); // Lista degli indirizzi associati al cliente
    }
}
