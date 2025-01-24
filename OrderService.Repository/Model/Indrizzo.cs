using System;

namespace OrderService.Repository.Model
{
    public class Indirizzo
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string Tipo { get; set; } // "Spedizione", "Fatturazione", ecc.
        public string IndirizzoCompleto { get; set; }
        public string NumeroCivico { get; set; }
        public string Cap { get; set; }
        public string Provincia { get; set; }
        public string Localita { get; set; }

        public Cliente Cliente { get; set; }
    }
}
