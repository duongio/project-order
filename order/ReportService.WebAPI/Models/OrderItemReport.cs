namespace ReportService.WebAPI.Models
{
    public class OrderItemReport
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderReport? OrderReport { get; set; }
    }
}
