namespace ReportService.WebAPI.Events.Models
{
    public class CreatePaymentSucceededEvent
    {
        public Guid OrderId { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
