namespace ReportService.WebAPI.Dtos
{
    public class ReportFilterDto
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string GroupBy { get; set; } = "day";
        public int Limit { get; set; } = int.MaxValue;
    }
}
