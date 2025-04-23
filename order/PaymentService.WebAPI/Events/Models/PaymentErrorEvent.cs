namespace PaymentService.WebAPI.Events.Models
{
    public class PaymentErrorEvent
    {
        public Guid OrderId { get; set; }
        public string Status { get; set; } = "Failed";
    }
}
