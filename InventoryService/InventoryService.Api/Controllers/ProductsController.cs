using InventoryService.Business.Abstraction; // Importa l'interfaccia della logica di business
using InventoryService.Shared.DTOs; // Importa i Data Transfer Object (DTO)
using Microsoft.AspNetCore.Mvc; // Importa i componenti per la creazione di API in ASP.NET Core

namespace InventoryService.Api.Controllers
{
    // Definisce il routing dell'API come "api/products"
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IInventoryBusiness _inventoryBusiness; // Istanza della logica di business

        // Costruttore che riceve la logica di business tramite Dependency Injection
        public ProductsController(IInventoryBusiness inventoryBusiness)
        {
            _inventoryBusiness = inventoryBusiness;
        }

        // Endpoint HTTP GET per ottenere tutti i prodotti
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
        {
            var products = await _inventoryBusiness.GetAllProductsAsync(); // Ottiene tutti i prodotti dal livello di business
            return Ok(products); // Restituisce lo stato HTTP 200 con la lista dei prodotti
        }

        // Endpoint HTTP GET per ottenere un prodotto specifico in base all'ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var product = await _inventoryBusiness.GetProductByIdAsync(id); // Ottiene il prodotto con l'ID specificato
            if (product == null) return NotFound(); // Restituisce HTTP 404 se il prodotto non esiste
            return Ok(product); // Restituisce HTTP 200 con il prodotto trovato
        }

        // Endpoint HTTP POST per aggiungere un nuovo prodotto
        [HttpPost]
        public async Task<ActionResult> AddProduct([FromBody] ProductDto productDto)
        {
            await _inventoryBusiness.AddProductAsync(productDto); // Chiama la logica di business per aggiungere il prodotto
            return CreatedAtAction(nameof(GetProductById), new { id = productDto.Id }, productDto); 
            // Restituisce HTTP 201 con il link al prodotto appena creato
        }
        
        // Endpoint HTTP POST per aggiornare lo stock di un prodotto
        [HttpPost("update-stock")]
        public async Task<ActionResult> UpdateStock([FromBody] ProductStockUpdateDto stockUpdateDto)
        {
            try
            {
                // Chiama la logica di business per aggiornare la quantità di stock
                await _inventoryBusiness.UpdateStockAsync(stockUpdateDto.ProductId, stockUpdateDto.Quantity);
                return Ok(); // Restituisce HTTP 200 se l'operazione è riuscita
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Restituisce HTTP 400 in caso di errore
            }
        }
    }
}
