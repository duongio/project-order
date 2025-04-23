using PaymentService.WebAPI.DTOs;
using PaymentService.WebAPI.Events.Interface;
using PaymentService.WebAPI.Events.Models;
using System.Net.Http;

namespace PaymentService.WebAPI.Services
{

    public class PaymentProcessor
    {
        private readonly IRabbitMQPublisher _publisher;
        private readonly HttpClient _httpClient;

        public PaymentProcessor(IRabbitMQPublisher publisher, IHttpClientFactory httpClientFactory)
        {
            _publisher = publisher;
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task ProcessPaymentAsync(OrderCreatedEvent order)
        {
            Console.WriteLine($"[PaymentService] Dang xu ly thanh toan cho don hang {order.Id}");

            var createPaymentDto = new CreatePaymentDto
            {
                OrderId = order.Id,
                Amount = order.TotalAmount,
                PIN = order.PIN,
            };

            var response = await _httpClient.PostAsJsonAsync("http://api-gateway/payments", createPaymentDto);

            if (response.IsSuccessStatusCode)
            {
                var payment = await response.Content.ReadFromJsonAsync<PaymentDto>();
                var evt = new PaymentSucceededEvent
                {
                    OrderId = order.Id,
                    Status = payment!.Status,
                };

                try
                {
                    _publisher.Publish(
                        evt,
                        exchangeName: "payment_exchange",
                        routingKey: "payment.succeeded"
                    );

                    Console.WriteLine("[PaymentService] Su kien da duoc publish thanh cong!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[PaymentService] Loi khi publish su kien: {ex.Message}");
                }

                Console.WriteLine($"[PaymentService] Thanh toan thanh cong cho don hang {order.Id}");
            }
            else
            {
                Console.WriteLine($"[PaymentService] Thanh toan that bai cho don hang {order.Id}");

                var payment = await response.Content.ReadFromJsonAsync<PaymentDto>();
                var evt = new PaymentSucceededEvent
                {
                    OrderId = order.Id,
                    Status = payment!.Status,
                };

                try
                {
                    _publisher.Publish(
                        evt,
                        exchangeName: "payment_exchange",
                        routingKey: "payment.failed"
                    );

                    Console.WriteLine("[PaymentService] Su kien da duoc publish thanh cong!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[PaymentService] Loi khi publish su kien: {ex.Message}");
                }
            }
        }
    }
}
