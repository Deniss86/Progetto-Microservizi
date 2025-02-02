namespace InventoryService.Shared.DTOs
{
    // DTO per rappresentare una richiesta di aggiornamento dello stock
    public class ProductStockUpdateDto
    {
        public int ProductId { get; set; } // Identificativo del prodotto da aggiornare
        public int Quantity { get; set; } // Quantità da aggiungere o rimuovere dallo stock
    }
}
//ProductStockUpdateDto è un DTO utilizzato per inviare richieste di aggiornamento dello stock di un prodotto.
//Viene usato nei controller API e nella logica di business per modificare lo stock dei prodotti senza esporre 
//direttamente il modello Product