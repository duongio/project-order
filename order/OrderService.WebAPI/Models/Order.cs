namespace OrderService.WebAPI.Models
{
    public enum OrderPaymentStatus
    {
        Pending,
        Succeeded,
        Failed
    }

    public class Order
    {
        public Guid Id { get; set; }
        public int CustomerId { get; set; }
        public OrderPaymentStatus Status { get; set; } = OrderPaymentStatus.Pending;
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt {  get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<OrderItem> Items { get; set; } = new();
    }
}
