namespace ReportService.WebAPI.Models
{
    public class PaymentReport
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
