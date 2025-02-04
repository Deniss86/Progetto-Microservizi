namespace OrderService.Repository.Model
{

    /*
    Questa classe viene utilizzata in OrderRepository.cs per memorizzare gli ordini nel database.
    È utilizzata in OrderBusiness.cs per gestire la logica di business degli ordini.
    */
    // Rappresenta un ordine nel sistema
    public class Order
    {
        public int Id { get; set; } // Identificativo univoco dell'ordine
        public int ProductId { get; set; } // ID del prodotto associato all'ordine
        public int Quantity { get; set; } // Quantità del prodotto ordinato
        public decimal TotalPrice { get; set; } // Prezzo totale dell'ordine
        public string Status { get; set; } = "Pending"; // Stato dell'ordine: "Pending", "Paid", "Shipped"
    }
}
