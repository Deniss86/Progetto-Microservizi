using OrderService.Business.Abstraction;
using OrderService.Repository.Abstraction;
using OrderService.Shared.Models;
using System.Text.Json;

namespace OrderService.Business
{
    public class OrderBusiness : IOrderBusiness
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IKafkaProducer _kafkaProducer;

        public OrderBusiness(IOrderRepository orderRepository, IKafkaProducer kafkaProducer)
        {
            _orderRepository = orderRepository;
            _kafkaProducer = kafkaProducer;
        }

        public async Task CreateOrderAsync(Order order)
        {
            if (order.Quantity <= 0)
                throw new ArgumentException("Invalid order quantity");

            await _orderRepository.AddOrderAsync(order);

            var outboxMessage = new OutboxMessageDto
            {
                Table = "Orders",
                Operation = "Insert",
                Message = JsonSerializer.Serialize(new
                {
                    Id = order.Id,
                    Quantity = order.Quantity,
                    TotalPrice = order.TotalPrice,
                    Status = order.Status
                })
            };

            await _orderRepository.AddOutboxMessageAsync(new TransactionalOutbox
            {
                Table = outboxMessage.Table,
                Operation = outboxMessage.Operation,
                Message = outboxMessage.Message
            });

            await _orderRepository.SaveChangesAsync();
        }

        public async Task<Order?> GetOrderAsync(int id)
        {
            return await _orderRepository.GetOrderByIdAsync(id);
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllOrdersAsync();
        }

        public async Task UpdateOrderAsync(int id, Order order)
        {
            var existingOrder = await _orderRepository.GetOrderByIdAsync(id);
            if (existingOrder == null)
                throw new KeyNotFoundException("Order not found");

            existingOrder.Quantity = order.Quantity;
            existingOrder.TotalPrice = order.TotalPrice;
            existingOrder.Status = order.Status;

            var outboxMessage = new OutboxMessageDto
            {
                Table = "Orders",
                Operation = "Update",
                Message = JsonSerializer.Serialize(new
                {
                    Id = existingOrder.Id,
                    Quantity = existingOrder.Quantity,
                    TotalPrice = existingOrder.TotalPrice,
                    Status = existingOrder.Status
                })
            };

            await _orderRepository.AddOutboxMessageAsync(new TransactionalOutbox
            {
                Table = outboxMessage.Table,
                Operation = outboxMessage.Operation,
                Message = outboxMessage.Message
            });

            await _orderRepository.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            await _orderRepository.RemoveAsync(id);

            var outboxMessage = new OutboxMessageDto
            {
                Table = "Orders",
                Operation = "Delete",
                Message = JsonSerializer.Serialize(new { Id = id })
            };

            await _orderRepository.AddOutboxMessageAsync(new TransactionalOutbox
            {
                Table = outboxMessage.Table,
                Operation = outboxMessage.Operation,
                Message = outboxMessage.Message
            });

            await _orderRepository.SaveChangesAsync();
        }

        public async Task ProcessOutboxMessagesAsync()
        {
            var messages = await _orderRepository.GetOutboxMessagesAsync();

            foreach (var message in messages)
            {
                var outboxDto = new OutboxMessageDto
                {
                    Table = message.Table,
                    Operation = message.Operation,
                    Message = message.Message
                };

                await _kafkaProducer.ProduceAsync(outboxDto.Table, JsonSerializer.Serialize(outboxDto));

                await _orderRepository.RemoveOutboxMessageAsync(message);
            }

            await _orderRepository.SaveChangesAsync();
        }
    }
}
