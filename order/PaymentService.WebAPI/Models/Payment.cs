namespace PaymentService.WebAPI.Models
{
    public enum StatusType
    {
        Pending,
        Succeeded,
        Failed
    }

    public class Payment
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public StatusType Status { get; set; } = StatusType.Pending;
    }
}
