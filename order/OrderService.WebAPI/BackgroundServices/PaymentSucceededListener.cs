using OrderService.WebAPI.BackgroundServices;
using OrderService.WebAPI.Events.Models;
using OrderService.WebAPI.Services;

public class PaymentSucceededListener : RabbitMqListenerService<PaymentSucceededEvent>
{
    public PaymentSucceededListener(IServiceProvider serviceProvider)
        : base(
            serviceProvider,
            queueName: "payment_succeeded_test_queue",
            exchangeName: "payment_exchange",
            routingKey: "payment.succeeded")
    {
    }

    protected override async Task ProcessMessageAsync(IServiceScope scope, PaymentSucceededEvent message)
    {
        Console.WriteLine($"[Consumer] Nhan duoc thanh toan: {message.OrderId}");

        var processor = scope.ServiceProvider.GetRequiredService<OrderProcessor>();
        await processor.UpdateOrderSuccess(message);
    }
}
