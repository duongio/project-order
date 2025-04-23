using ReportService.WebAPI.Events.Models;
using ReportService.WebAPI.Services;

namespace ReportService.WebAPI.BackgroundServices
{
    public class CreateOrderSucceededListener : RabbitMqListenerService<CreateOrderSucceededEvent>
    {
        public CreateOrderSucceededListener(IServiceProvider serviceProvider)
            : base(
                serviceProvider,
                queueName: "order_created_report_test_queue",
                exchangeName: "order_exchange",
                routingKey: "order.created")
        {
        }

        protected override async Task ProcessMessageAsync(IServiceScope scope, CreateOrderSucceededEvent message)
        {
            Console.WriteLine($"[Consumer] Nhan duoc don hang moi: {message.Id}");

            var processor = scope.ServiceProvider.GetRequiredService<ReportProcessor>();
            await processor.AddOrderReport(message);
        }
    }
}
