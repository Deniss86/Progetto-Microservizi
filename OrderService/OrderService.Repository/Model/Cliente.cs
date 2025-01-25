using System;
using System.Collections.Generic;

namespace OrderService.Repository.Model
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Email { get; set; }
        public string CodiceFiscale { get; set; }
        public DateTime DataDiNascita { get; set; }

        public List<Indirizzo> Indirizzi { get; set; } = new List<Indirizzo>();
    }
}
