using InventoryService.Business.Abstraction;
using InventoryService.Repository.Abstraction;
using InventoryService.Shared.DTOs;
using InventoryService.Repository.Model;
using InventoryService.Kafka.Abstraction;
using System.Text.Json;

namespace InventoryService.Business
{
    public class InventoryBusiness : IInventoryBusiness
    {
        private readonly IProductRepository _productRepository;
        private readonly IKafkaProducer _kafkaProducer;

        public InventoryBusiness(IProductRepository productRepository, IKafkaProducer kafkaProducer)
        {
            _productRepository = productRepository;
            _kafkaProducer = kafkaProducer;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Stock = p.Stock,
                Price = p.Price
            });
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null) return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Stock = product.Stock,
                Price = product.Price
            };
        }

        public async Task AddProductAsync(ProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Stock = productDto.Stock,
                Price = productDto.Price
            };

            await _productRepository.AddProductAsync(product);

            var outboxMessage = new OutboxMessageDto
            {
                Table = "Products",
                Operation = "Insert",
                Message = JsonSerializer.Serialize(new
                {
                    Id = product.Id,
                    Name = product.Name,
                    Stock = product.Stock,
                    Price = product.Price
                })
            };

            await _productRepository.AddOutboxMessageAsync(new TransactionalOutbox
            {
                Table = outboxMessage.Table,
                Operation = outboxMessage.Operation,
                Message = outboxMessage.Message
            });

            await _productRepository.SaveChangesAsync();
        }

        public async Task UpdateStockAsync(int productId, int quantity)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null)
            {
                throw new Exception($"Product with ID {productId} not found.");
            }

            product.Stock -= quantity;

            var outboxMessage = new OutboxMessageDto
            {
                Table = "Products",
                Operation = "Update",
                Message = JsonSerializer.Serialize(new
                {
                    Id = product.Id,
                    Name = product.Name,
                    Stock = product.Stock,
                    Price = product.Price
                })
            };

            await _productRepository.AddOutboxMessageAsync(new TransactionalOutbox
            {
                Table = outboxMessage.Table,
                Operation = outboxMessage.Operation,
                Message = outboxMessage.Message
            });

            await _productRepository.SaveChangesAsync();
        }

        public async Task ProcessOutboxMessagesAsync()
        {
            var messages = await _productRepository.GetOutboxMessagesAsync();

            foreach (var message in messages)
            {
                var outboxDto = new OutboxMessageDto
                {
                    Table = message.Table,
                    Operation = message.Operation,
                    Message = message.Message
                };

                await _kafkaProducer.ProduceAsync(outboxDto.Table, JsonSerializer.Serialize(outboxDto));

                await _productRepository.RemoveOutboxMessageAsync(message);
            }

            await _productRepository.SaveChangesAsync();
        }
    }
}
