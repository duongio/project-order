namespace ReportService.WebAPI.Dtos
{
    public class OverviewDto
    {
        public DateTime Date { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int TotalCustomers { get; set; }
        public decimal AverageOrderValue { get; set; }
    }
}
