namespace ReportService.WebAPI.Dtos
{
    public class PaymentReportDto
    {
        public Guid OrderId { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
