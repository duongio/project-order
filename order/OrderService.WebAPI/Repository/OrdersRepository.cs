using Microsoft.EntityFrameworkCore;
using OrderService.WebAPI.Data;
using OrderService.WebAPI.DTOs;
using OrderService.WebAPI.Models;

namespace OrderService.WebAPI.Repository
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly OrderDbContext _context;

        public OrdersRepository(OrderDbContext context) 
        {
            _context = context;
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto orderDto)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = orderDto.CustomerId,
                Status = OrderPaymentStatus.Pending,
                TotalAmount = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            var orderItems = orderDto.Items.Select(itemDto => new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                ProductId = itemDto.ProductId,
                Quantity = itemDto.Quantity,
                UnitPrice = itemDto.UnitPrice,
                TotalPrice = itemDto.Quantity * itemDto.UnitPrice
            }).ToList();
            order.Items.AddRange(orderItems);
            order.TotalAmount = order.Items.Sum(item => item.TotalPrice);
            _context.Orders.Add(order);
            _context.OrderItems.AddRange(orderItems);
            await _context.SaveChangesAsync();
            var orderDtoRes = new OrderDto
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                Status = order.Status.ToString(),
                TotalAmount = order.TotalAmount,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                Items = order.Items.Select(item => new OrderItemDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList(),
                PIN = orderDto.PIN
            };
            return orderDtoRes;
        }

        public async Task DeleteOrderAsync(Guid id)
        {
            var order = await _context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) { throw new KeyNotFoundException("Order not found"); }
            if (order.Items.Any()) 
            {
                _context.OrderItems.RemoveRange(order.Items);
            }
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _context.Orders.Include(o => o.Items).ToListAsync();
            var orderDtos = orders.Select(order => new OrderDto
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                Status = order.Status.ToString(),
                TotalAmount = order.TotalAmount,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                Items = order.Items.Select(item => new OrderItemDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList(),
            }).ToList();
            return orderDtos;
        }

        public async Task<OrderDto?> GetOrderByIdAsync(Guid id)
        {
            var order = await _context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                return null;
            }
            var orderDto = new OrderDto
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                Status = order.Status.ToString(),
                TotalAmount = order.TotalAmount,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                Items = order.Items.Select(item => new OrderItemDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList(),
            };
            return orderDto;
        }

        public async Task UpdateOrderAsync(UpdateOrderDto updateOrder)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == updateOrder.Id);
            if (order == null) { throw new KeyNotFoundException("Order not found"); }
            if (!Enum.TryParse<OrderPaymentStatus>(updateOrder.Status, true, out var status)) { throw new ArgumentException("Invalid order status"); }
            order.Status = status;
            order.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}
