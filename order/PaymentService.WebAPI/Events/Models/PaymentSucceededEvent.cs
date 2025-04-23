namespace PaymentService.WebAPI.Events.Models
{
    public class PaymentSucceededEvent
    {
        public Guid OrderId { get; set; }
        public string Status { get; set; } = "Succeeded";
    }
}
