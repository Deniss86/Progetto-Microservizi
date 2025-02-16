﻿using InventoryService.Business.Abstraction; // Importa l'interfaccia della logica di business
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
            _productRepository = productRepository; // Inizializza il repository
        }

        // Metodo per ottenere tutti i prodotti

        // Si basa su getAllProductsAsync() definito in InventoryService.Repository/Abstraction/IProductRepository.cs
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
        // Si basa su getProductByIdAsync() definito in InventoryService.Repository/Abstraction/IProductRepository.cs
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
        // Si basa su addProductAsync() definito in InventoryService.Repository/Abstraction/IProductRepository.cs
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
            // Recupera il prodotto dal repository tramite l'ID
            var product = await _productRepository.GetProductByIdAsync(productId);
            // Solleva un'eccezione se il prodotto non esiste
            if (product == null)
            {
                throw new Exception($"Product with ID {productId} not found.");
            }
            // Solleva un'eccezione se la quantità è minore di 0
            if (product.Stock < quantity)
            {
                throw new Exception("Stock insufficiente per completare l'operazione.");
            }
            // Aggiorna la quantità di stock del prodotto
            product.Stock -= quantity;
            // Salva le modifiche nel database
            await _productRepository.SaveChangesAsync();
        }

        // Metodo per rimuovere un prodotto dall'inventario
        public async Task RemoveProductAsync(int id)
        {
            // Recupera il prodotto dal repository tramite l'ID
            var product = await _productRepository.GetProductByIdAsync(id); // Ottiene il prodotto dal repository
            // Solleva un'eccezione se il prodotto non esiste
            if (product == null)
            {
                throw new Exception($"Product with ID {id} not found."); // Solleva un'eccezione se il prodotto non esiste
            }
            //  Rimuove il prodotto dal repository e assicura che il database venga aggiornato
            await _productRepository.RemoveAsync(id); // Rimuove il prodotto dal repository
            await _productRepository.SaveChangesAsync(); //  Assicura che il database venga aggiornato
        }


    }
}
