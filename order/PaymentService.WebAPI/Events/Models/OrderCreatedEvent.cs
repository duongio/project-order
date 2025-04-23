namespace PaymentService.WebAPI.Events.Models
{
    public class OrderCreatedEvent
    {
        public Guid Id { get; set; }
        public decimal TotalAmount { get; set; }
        public int PIN { get; set; }
    }
}
