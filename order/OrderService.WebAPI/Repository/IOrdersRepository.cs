using OrderService.WebAPI.DTOs;

namespace OrderService.WebAPI.Repository
{
    public interface IOrdersRepository
    {
        Task<List<OrderDto>> GetAllOrdersAsync();
        Task<OrderDto?> GetOrderByIdAsync(Guid id);
        Task<OrderDto> CreateOrderAsync(CreateOrderDto orderDto);
        Task UpdateOrderAsync(UpdateOrderDto updateOrder);
        Task DeleteOrderAsync(Guid id);
    }
}
