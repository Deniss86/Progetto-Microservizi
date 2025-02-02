namespace OrderService.Shared.Models
{
    // DTO per rappresentare un prodotto
    // Questo DTO è utilizzato nei controller API e nel ClientHttp.cs per comunicare con il servizio di inventario.
    public class ProductDto
    {
        public int Id { get; set; } // Identificativo del prodotto
        public string Name { get; set; } = string.Empty; // Nome del prodotto
        public decimal Price { get; set; } // Prezzo del prodotto
        public int StockQuantity { get; set; } // Quantità disponibile in magazzino
    }
}
