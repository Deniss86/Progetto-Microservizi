namespace InventoryService.Repository.Model
{
    // Rappresenta un prodotto nel database
    public class Product
    {
        public int Id { get; set; } // Identificativo univoco del prodotto
        public required string Name { get; set; } // Nome del prodotto (campo obbligatorio)
        public int Stock { get; set; } // Quantità in stock del prodotto
        public decimal Price { get; set; } // Prezzo del prodotto
    }
}
