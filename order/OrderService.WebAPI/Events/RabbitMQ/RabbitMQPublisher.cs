using OrderService.WebAPI.Events.Interface;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace OrderService.WebAPI.Events.RabbitMQ
{
    public class RabbitMQPublisher : IRabbitMQPublisher
    {
        private readonly string _hostname = "rabbitmq";

        public RabbitMQPublisher(IConfiguration configuration)
        {
            _hostname = configuration["RabbitMQ:HostName"] ?? "rabbitmq";
        }

        public void Publish<T>(T message, string exchangeName, string routingKey)
        {
            try
            {
                var factory = new ConnectionFactory { HostName = _hostname };
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct, durable: true);

                var json = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: exchangeName, routingKey: routingKey, basicProperties: null, body: body);

                Console.WriteLine($"[Publisher] Message publish len exchange '{exchangeName}' voi routingKey '{routingKey}': {json}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Publisher] Error: {ex.Message}");
            }
        }

    }
}
