namespace ReportService.WebAPI.Events.Models
{
    public class CreateOrderItemSucceededEvent
    {
        public Guid Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
