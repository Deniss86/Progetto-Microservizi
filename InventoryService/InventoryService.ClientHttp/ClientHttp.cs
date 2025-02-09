﻿using InventoryService.ClientHttp.Abstraction; // Importa l'interfaccia per la comunicazione HTTP
using InventoryService.Shared.DTOs; // Importa i DTO
using System; // Importa il namespace System
using System.Net.Http; // Importa la classe HttpClient per effettuare richieste HTTP
using System.Net.Http.Json; // Importa i metodi helper per la serializzazione JSON
using System.Threading; // Importa la gestione dei token di cancellazione
using System.Threading.Tasks; // Importa il supporto per le operazioni asincrone

// Implementazione della comunicazione HTTP con i servizi remoti

namespace InventoryService.ClientHttp
{
    // Implementa l'interfaccia IClientHttp per la comunicazione con servizi remoti
    public class ClientHttp : IClientHttp
    {
        private readonly HttpClient _httpClient; // Istanza di HttpClient per effettuare richieste HTTP

        // Costruttore che riceve un'istanza di HttpClient tramite Dependency Injection
        public ClientHttp(HttpClient httpClient)
        {
            _httpClient = httpClient; // Inizializza l'istanza di HttpClient
        }
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // Questi metodi dentro Inventory service non servono a niente, sono gia implementati in Order
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!



        // Metodo per recuperare un prodotto dal servizio remoto tramite il suo ID
        public async Task<ProductDto?> GetProductByIdAsync(int productId, CancellationToken cancellationToken = default)
        {
            try
            {
                // Effettua una richiesta GET per ottenere i dettagli del prodotto
                return await _httpClient.GetFromJsonAsync<ProductDto>($"api/products/{productId}", cancellationToken);
            }
            catch (Exception)
            {
                // Se si verifica un errore (es. servizio non disponibile), restituisce null
                return null;
            }
        }
        
        // Metodo per aggiornare lo stock di un prodotto tramite una richiesta HTTP POST
        public async Task<bool> UpdateStockAsync(ProductStockUpdateDto stockUpdate, CancellationToken cancellationToken = default)
        {
            try
            {
                // Effettua una richiesta POST per aggiornare lo stock del prodotto
                var response = await _httpClient.PostAsJsonAsync("api/products/update-stock", stockUpdate, cancellationToken);
                
                // Restituisce true se la richiesta ha avuto successo, altrimenti false
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                // Se si verifica un errore (es. connessione fallita), restituisce false
                return false;
            }
        }
    }
}
