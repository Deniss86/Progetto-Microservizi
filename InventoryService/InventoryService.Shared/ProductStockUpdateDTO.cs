namespace InventoryService.Shared.DTOs
{
    public class ProductStockUpdateDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; } // Quantità da aggiungere o rimuovere dallo stock
    }
}
