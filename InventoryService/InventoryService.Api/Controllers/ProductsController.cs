using InventoryService.Business.Abstraction; // Importa l'interfaccia della logica di business
using InventoryService.Shared.DTOs; // Importa i Data Transfer Object (DTO)
using Microsoft.AspNetCore.Mvc; // Importa i componenti per la creazione di API in ASP.NET Core

namespace InventoryService.Api.Controllers
{
    // Definisce il routing dell'API come "api/products"
    [Route("api/[controller]/[action]")] // Routing
    [ApiController] // Indica che la classe è un controller API
    public class ProductsController : ControllerBase
    {
        private readonly IInventoryBusiness _inventoryBusiness; // Istanza della logica di business

        // Costruttore che riceve la logica di business tramite Dependency Injection
        public ProductsController(IInventoryBusiness inventoryBusiness)
        {
            _inventoryBusiness = inventoryBusiness; // Inizializza la logica di business
        }

        // Endpoint HTTP GET per ottenere tutti i prodotti
        // Si basa su getAllProductsAsync() definito in InventoryService.Business/Abstraction/IInventoryBusiness.cs
        [HttpGet (Name = "GetAllProducts")] // Routing

        // Task indica che il metodo è asincrono, IEnumerable è una collezione di elementi
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()  // Action
        {
            var products = await _inventoryBusiness.GetAllProductsAsync(); // Ottiene tutti i prodotti dal livello di business
            return Ok(products); // Restituisce lo stato HTTP 200 con la lista dei prodotti
        }

        // Endpoint HTTP GET per ottenere un prodotto specifico in base all'ID
        // Si basa su getProductByIdAsync() definito in InventoryService.Business/Abstraction/IInventoryBusiness.cs
        [HttpGet("{id}", Name = "GetProductsByID")] // Routing
        public async Task<ActionResult<ProductDto>> GetProductById(int id) // Action
        {
            var product = await _inventoryBusiness.GetProductByIdAsync(id); // Ottiene il prodotto con l'ID specificato
            if (product == null) return NotFound(); // Restituisce HTTP 404 se il prodotto non esiste
            return Ok(product); // Restituisce HTTP 200 con il DTO in JSON
        }

        // Endpoint HTTP POST per aggiungere un nuovo prodotto
        // Si basa su addProductAsync() definito in InventoryService.Business/Abstraction/IInventoryBusiness.cs
        [HttpPost(Name = "AddProduct")] // Routing
        public async Task<ActionResult> AddProduct([FromBody] ProductDto productDto) // Action
        {
            await _inventoryBusiness.AddProductAsync(productDto); // Chiama la logica di business per aggiungere il prodotto
            // Restituisce HTTP 201 (Creato) con il link al prodotto appena creato
            return CreatedAtAction(nameof(GetProductById), new { id = productDto.Id }, productDto); 
            // Restituisce HTTP 201 con il link al prodotto appena creato
        }
        
        // Endpoint HTTP POST per aggiornare lo stock di un prodotto
        // Si basa su updateStockAsync() definito in InventoryService.Business/Abstraction/IInventoryBusiness.cs
        [HttpPost("update-stock", Name = "UpdateStock")] // Routing 
        public async Task<ActionResult> UpdateStock([FromBody] ProductStockUpdateDto stockUpdateDto) // Action
        {
            try
            {   // Stampa un messaggio sulla console con i dati ricevuti
                Console.WriteLine($"UpdateStock called with ProductId: {stockUpdateDto.ProductId}, Quantity: {stockUpdateDto.Quantity}");
                
                // Chiama la logica di business per aggiornare la quantità di stock
                await _inventoryBusiness.UpdateStockAsync(stockUpdateDto.ProductId, stockUpdateDto.Quantity);
                // Stampa un messaggio sulla console con l'esito dell'operazione
                Console.WriteLine($"Stock updated for ProductId: {stockUpdateDto.ProductId}");
                return Ok(); // Restituisce HTTP 200 se l'operazione è riuscita
            }
            catch (Exception ex) // Cattura eventuali eccezioni
            {
                return BadRequest(ex.Message); // Restituisce HTTP 400 in caso di errore con il messaggio dell'eccezione
            }
        }

        // Endpoint HTTP DELETE per eliminare un prodotto
        [HttpDelete("{id}", Name = "DeleteProduct")] // Routing
        public async Task<IActionResult> DeleteProduct(int id) // Action
        {
            try
            {
                // Chiama la logica di business per eliminare il prodotto
                await _inventoryBusiness.RemoveProductAsync(id);
                return NoContent(); // Restituisce HTTP 204 (Eliminazione avvenuta con successo)
            } 
            catch (Exception ex) // Cattura eventuali eccezioni   
            {
                return NotFound(new { message = ex.Message }); // Restituisce HTTP 404 se il prodotto non esiste
            }
        }

    }
}
