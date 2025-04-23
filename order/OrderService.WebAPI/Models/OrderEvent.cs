using System.ComponentModel.DataAnnotations;

namespace OrderService.WebAPI.Models
{
    public enum OrderEventType
    {
        Created,
        StatusChanged,
        Cancelled,
        PaymentConfirmed,
        Shipped
    }

    public enum EventStatus
    {
        Pending,
        Sent,
        Failed,
        Retrying
    }

    public class OrderEvent
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public OrderEventType EventType { get; set; } = OrderEventType.Created;
        public string Payload { get; set; } = string.Empty; // raw JSON
        public EventStatus Status { get; set; } = EventStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
