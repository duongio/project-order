namespace OrderService.WebAPI.DTOs
{
    public class UpdateOrderDto
    {
        public Guid Id { get; set; }
        public string Status { get; set; } = String.Empty;
    }
}
