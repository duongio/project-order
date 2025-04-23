using PaymentService.WebAPI.BackgroundServices;
using PaymentService.WebAPI.Events.Models;
using PaymentService.WebAPI.Services;

public class OrderCreatedListenerService : RabbitMqListenerService<OrderCreatedEvent>
{
    public OrderCreatedListenerService(IServiceProvider serviceProvider)
        : base(
            serviceProvider,
            queueName: "order_created_test_queue",
            exchangeName: "order_exchange",
            routingKey: "order.created")
    {
    }

    protected override async Task ProcessMessageAsync(IServiceScope scope, OrderCreatedEvent message)
    {
        Console.WriteLine($"[Consumer] Nhan duoc don hang: {message.Id}");

        var processor = scope.ServiceProvider.GetRequiredService<PaymentProcessor>();
        await processor.ProcessPaymentAsync(message);
    }
}
