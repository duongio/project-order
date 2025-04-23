using OrderService.WebAPI.Models;

namespace OrderService.WebAPI.DTOs
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public int CustomerId { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
        public int PIN { get; set; }
    }
}
