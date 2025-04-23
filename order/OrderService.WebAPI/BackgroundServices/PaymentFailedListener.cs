using OrderService.WebAPI.Events.Models;
using OrderService.WebAPI.Services;

namespace OrderService.WebAPI.BackgroundServices
{
    public class PaymentFailedListener : RabbitMqListenerService<PaymentErrorEvent>
    {
        public PaymentFailedListener(IServiceProvider serviceProvider)
            : base(
                serviceProvider,
                queueName: "payment_failed_test_queue",
                exchangeName: "payment_exchange",
                routingKey: "payment.failed")
        {
        }

        protected override async Task ProcessMessageAsync(IServiceScope scope, PaymentErrorEvent message)
        {
            Console.WriteLine($"[Consumer] Nhan duoc thanh toan: {message.OrderId}");

            var processor = scope.ServiceProvider.GetRequiredService<OrderProcessor>();
            await processor.UpdateOrderFailed(message);
        }
    }
}
