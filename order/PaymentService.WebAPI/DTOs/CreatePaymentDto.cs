using PaymentService.WebAPI.Models;

namespace PaymentService.WebAPI.DTOs
{
    public enum StatusType
    {
        Pending,
        Succeeded,
        Failed
    }

    public class CreatePaymentDto
    {
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }
        public int PIN { get; set; }
        public StatusType Status { get; set; } = StatusType.Pending;
    }
}
