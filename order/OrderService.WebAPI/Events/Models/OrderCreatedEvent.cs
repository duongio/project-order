namespace OrderService.WebAPI.Events.Models
{
    public class OrderCreatedEvent
    {
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }
    }
}
