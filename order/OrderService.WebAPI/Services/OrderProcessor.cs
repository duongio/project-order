using OrderService.WebAPI.DTOs;
using OrderService.WebAPI.Events.Interface;
using OrderService.WebAPI.Events.Models;
using System.Net.Http;

namespace OrderService.WebAPI.Services
{
    public class OrderProcessor
    {
        private readonly IRabbitMQPublisher _publisher;
        private readonly HttpClient _httpClient;

        public OrderProcessor(IRabbitMQPublisher publisher, IHttpClientFactory httpClientFactory)
        {
            _publisher = publisher;
            _httpClient = httpClientFactory.CreateClient();
        }

        public void CreateOrder(OrderDto order)
        {
            Console.WriteLine($"[OrderProcessor] Don hang {order.Id} duoc tao!");

            var evt = order;

            try
            {
                _publisher.Publish(
                    evt,
                    exchangeName: "order_exchange",
                    routingKey: "order.created"
                );

                Console.WriteLine("[OrderProcessor] Su kien da duoc publish thanh cong!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[OrderProcessor] Loi khi publish su kien: {ex.Message}");
            }
        }


        public async Task UpdateOrderSuccess(PaymentSucceededEvent order)
        {
            Console.WriteLine($"[OrderProcessor] Dang xu ly trag thai thanh toan cho don hang {order.OrderId}");

            var updateOrderDto = new UpdateOrderDto
            {
                Id = order.OrderId,
                Status = order.Status
            };

            var response = await _httpClient.PutAsJsonAsync($"http://api-gateway/orders/{updateOrderDto.Id}", updateOrderDto);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[OrderService] Cap nhat trang thai thanh toan cho don hang {order.OrderId} thanh cong");
            }
            else
            {
                Console.WriteLine($"[OrderService] Cap nhat trang thai thanh toan cho don hang {order.OrderId} that bai");
            }
        }

        public async Task UpdateOrderFailed(PaymentErrorEvent order)
        {
            Console.WriteLine($"[OrderProcessor] Dang xu ly trag thai thanh toan cho don hang {order.OrderId}");

            var updateOrderDto = new UpdateOrderDto
            {
                Id = order.OrderId,
                Status = order.Status
            };

            var response = await _httpClient.PutAsJsonAsync($"http://api-gateway/orders/{updateOrderDto.Id}", updateOrderDto);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[OrderService] Cap nhat trang thai thanh toan cho don hang {order.OrderId} thanh cong");
            }
            else
            {
                Console.WriteLine($"[OrderService] Cap nhat trang thai thanh toan cho don hang {order.OrderId} that bai");
            }
        }
    }
}
