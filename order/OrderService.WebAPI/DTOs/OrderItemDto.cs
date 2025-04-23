namespace OrderService.WebAPI.DTOs
{
    public class OrderItemDto
    {
        public Guid Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
