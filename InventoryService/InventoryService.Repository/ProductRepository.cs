using InventoryService.Repository.Abstraction; // Importa l'interfaccia del repository
using InventoryService.Repository.Model; // Importa i modelli delle entità
using Microsoft.EntityFrameworkCore; // Importa Entity Framework Core
using System.Collections.Generic; // Importa il supporto per le collezioni
using System.Threading.Tasks; // Importa il supporto per le operazioni asincrone

namespace InventoryService.Repository
{
    // Implementa il repository per la gestione dei prodotti
    public class ProductRepository : IProductRepository
    {
        private readonly InventoryDbContext _dbContext; // Istanza del contesto database

        // Costruttore che riceve il contesto database tramite Dependency Injection
        public ProductRepository(InventoryDbContext dbContext)
        {
            _dbContext = dbContext; // Inizializza il contesto database
        }

        // Ottiene un prodotto tramite ID
        public async Task<Product> GetProductByIdAsync(int id)
        {   
            // Cerca il prodotto con l'ID specificato
            return await _dbContext.Products.FindAsync(id);
        }

        // Ottiene tutti i prodotti
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            // Restituisce tutti i prodotti presenti nel database
            return await _dbContext.Products.ToListAsync();
        }

        // Aggiunge un nuovo prodotto
        public async Task AddProductAsync(Product product)
        {
            // Aggiunge il prodotto al database
            await _dbContext.Products.AddAsync(product);
        }

        // Salva le modifiche nel database
        public async Task<bool> SaveChangesAsync()
        {
            // Restituisce true se almeno un'entità è stata modificata
            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        // Rimuove un prodotto dal database
        public async Task RemoveAsync(int id)
        {   
            // Cerca il prodotto con l'ID specificato
            var product = await GetProductByIdAsync(id);

            // Se il prodotto esiste, lo rimuove dal database
            if (product != null)
            {
                _dbContext.Products.Remove(product);
            }
        }
    }
}
