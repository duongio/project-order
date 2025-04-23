using ReportService.WebAPI.Dtos;

namespace ReportService.WebAPI.Events.Models
{
    public class CreateOrderSucceededEvent
    {
        public Guid Id { get; set; }
        public int CustomerId { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<OrderItemReportDto> Items { get; set; } = new();
    }
}
