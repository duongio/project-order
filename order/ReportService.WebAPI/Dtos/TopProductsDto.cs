namespace ReportService.WebAPI.Dtos
{
    public class TopProductsDto
    {
        public int ProductId { get; set; }
        public int QuantitySold { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
