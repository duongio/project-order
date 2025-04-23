using OrderService.WebAPI.Events.RabbitMQ;

namespace OrderService.WebAPI.Events.Interface
{
    public interface IRabbitMQPublisher
    {
        void Publish<T>(T message, string exchangeName, string routingKey);
    }
}
