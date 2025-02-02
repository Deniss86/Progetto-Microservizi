using InventoryService.Business.Abstraction; // Importa l'interfaccia della logica di business
using InventoryService.Repository.Abstraction; // Importa l'interfaccia del repository
using InventoryService.Shared.DTOs; // Importa i Data Transfer Object (DTO)
using InventoryService.Repository.Model; // Importa i modelli delle entità del database
using System.Text.Json; // Importa la libreria per la serializzazione JSON

namespace InventoryService.Business
{
    // Implementazione della logica di business per la gestione dell'inventario
    public class InventoryBusiness : IInventoryBusiness
    {
        private readonly IProductRepository _productRepository; // Istanza del repository dei prodotti

        // Costruttore con Dependency Injection del repository
        public InventoryBusiness(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // Metodo per ottenere tutti i prodotti
        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllProductsAsync(); // Recupera tutti i prodotti dal repository
            
            // Converte la lista di prodotti nel formato DTO (Data Transfer Object)
            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Stock = p.Stock,
                Price = p.Price
            });
        }

        // Metodo per ottenere un prodotto tramite il suo ID
        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id); // Recupera il prodotto dal repository
            
            if (product == null) return null; // Se il prodotto non esiste, restituisce null

            // Converte l'entità in DTO
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Stock = product.Stock,
                Price = product.Price
            };
        }

        // Metodo per aggiungere un nuovo prodotto all'inventario
        public async Task AddProductAsync(ProductDto productDto)
        {
            // Crea un nuovo oggetto Product basato sui dati ricevuti
            var product = new Product
            {
                Name = productDto.Name,
                Stock = productDto.Stock,
                Price = productDto.Price
            };

            await _productRepository.AddProductAsync(product); // Aggiunge il prodotto al database
            await _productRepository.SaveChangesAsync(); // Salva le modifiche nel database
        }

        // Metodo per aggiornare la quantità di stock di un prodotto
        public async Task UpdateStockAsync(int productId, int quantity)
        {
            var product = await _productRepository.GetProductByIdAsync(productId); // Recupera il prodotto
            
            if (product == null)
            {
                throw new Exception($"Product with ID {productId} not found."); // Se il prodotto non esiste, genera un'eccezione
            }

            product.Stock -= quantity; // Aggiorna la quantità disponibile
            await _productRepository.SaveChangesAsync(); // Salva le modifiche nel database
        }
    }
}
