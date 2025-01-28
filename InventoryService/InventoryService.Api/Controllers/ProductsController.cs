using InventoryService.Business.Abstraction;
using InventoryService.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IInventoryBusiness _inventoryBusiness;

        public ProductsController(IInventoryBusiness inventoryBusiness)
        {
            _inventoryBusiness = inventoryBusiness;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
        {
            var products = await _inventoryBusiness.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var product = await _inventoryBusiness.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct([FromBody] ProductDto productDto)
        {
            await _inventoryBusiness.AddProductAsync(productDto);
            return CreatedAtAction(nameof(GetProductById), new { id = productDto.Id }, productDto);
        }
        /*
        [HttpPost("update-stock")]
        public async Task<ActionResult> UpdateStock([FromBody] ProductStockUpdateDto stockUpdateDto)
        {
            try
            {
                await _inventoryBusiness.UpdateStockAsync(stockUpdateDto.ProductId, stockUpdateDto.Quantity);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } */
    }
}
