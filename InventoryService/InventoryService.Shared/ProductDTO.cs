namespace InventoryService.Shared.DTOs
{
    // DTO per rappresentare un prodotto nel sistema
    public class ProductDto
    {
        public int Id { get; set; } // Identificativo del prodotto
        public required string Name { get; set; } // Nome del prodotto (campo obbligatorio)
        public int Stock { get; set; } // Quantità disponibile in magazzino
        public decimal Price { get; set; } // Prezzo del prodotto
    }
}
//ProductDto è un Data Transfer Object (DTO) che rappresenta un prodotto nelle API.
/*Questo DTO viene usato per trasferire i dati dei prodotti tra il backend e i client (es. API).
È usato nei controller, nel repository e nel livello di business per evitare di esporre direttamente 
il modello del database Product.cs.
*/