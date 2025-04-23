using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace PaymentService.WebAPI.BackgroundServices
{
    public abstract class RabbitMqListenerService<T> : BackgroundService
    {
        private readonly string _queueName;
        private readonly string _exchangeName;
        private readonly string _routingKey;
        private readonly IServiceProvider _serviceProvider;

        protected RabbitMqListenerService(
            IServiceProvider serviceProvider,
            string queueName,
            string exchangeName,
            string routingKey)
        {
            _queueName = queueName;
            _exchangeName = exchangeName;
            _routingKey = routingKey;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = "rabbitmq",
                DispatchConsumersAsync = true
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Direct, durable: true);
            channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false);
            channel.QueueBind(queue: _queueName, exchange: _exchangeName, routingKey: _routingKey);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var json = Encoding.UTF8.GetString(body);
                    var message = JsonSerializer.Deserialize<T>(json);

                    if (message != null)
                    {
                        using var scope = _serviceProvider.CreateScope();
                        await ProcessMessageAsync(scope, message);
                    }

                    channel.BasicAck(ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Loi xu ly message: {ex}");
                    channel.BasicNack(ea.DeliveryTag, false, true);
                }
            };

            channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        protected abstract Task ProcessMessageAsync(IServiceScope scope, T message);
    }

}
