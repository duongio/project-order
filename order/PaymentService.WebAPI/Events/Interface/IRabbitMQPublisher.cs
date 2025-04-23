using PaymentService.WebAPI.Events.RabbitMQ;

namespace PaymentService.WebAPI.Events.Interface
{
    public interface IRabbitMQPublisher
    {
        void Publish<T>(T message, string exchangeName, string routingKey);
    }
}
