namespace OrderService.Shared.Models
{
    public class ProductStockUpdateDto
    {
        public int ProductId { get; set; }
        public int QuantityToUpdate { get; set; } // Positivo per aggiungere, negativo per rimuovere
    }
}
