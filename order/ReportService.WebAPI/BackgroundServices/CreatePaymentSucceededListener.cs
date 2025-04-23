using ReportService.WebAPI.Events.Models;
using ReportService.WebAPI.Services;

namespace ReportService.WebAPI.BackgroundServices
{
    public class CreatePaymentSucceededListener : RabbitMqListenerService<CreatePaymentSucceededEvent>
    {
        public CreatePaymentSucceededListener(IServiceProvider serviceProvider)
            : base(
                serviceProvider,
                queueName: "payment_succeeded_report_test_queue",
                exchangeName: "payment_exchange",
                routingKey: "payment.succeeded")
        {
        }

        protected override async Task ProcessMessageAsync(IServiceScope scope, CreatePaymentSucceededEvent message)
        {
            Console.WriteLine($"[Consumer] Nhan duoc trang thai thanh toan cua don hang {message.OrderId}");

            var processor = scope.ServiceProvider.GetRequiredService<ReportProcessor>();
            await processor.AddPaymentReport(message);
        }
    }
}
