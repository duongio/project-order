using ReportService.WebAPI.Events.Models;

namespace ReportService.WebAPI.Services
{
    public class ReportProcessor
    {
        private readonly HttpClient _httpClient;

        public ReportProcessor(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task AddOrderReport(CreateOrderSucceededEvent order)
        {
            Console.WriteLine($"[ReportProccessor] Dang them don hang {order.Id}");

            var response = await _httpClient.PostAsJsonAsync($"http://api-gateway/reports/Order", order);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[ReportProccessor] Them thanh cong don hang {order.Id}");
            }
            else
            {
                Console.WriteLine($"[ReportProccessor] Them don hang {order.Id} khong thanh cong");
            }
        }
        public async Task AddPaymentReport(CreatePaymentSucceededEvent payment)
        {
            Console.WriteLine($"[ReportProccessor] Dang them trang thai thanh toan cho don hang {payment.OrderId}");

            var response = await _httpClient.PostAsJsonAsync($"http://api-gateway/reports/Payment", payment);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[ReportProccessor] Them thanh cong trang thai thanh toan don hang {payment.OrderId}");
            }
            else
            {
                Console.WriteLine($"[ReportProccessor] Them trang thai thanh toan don hang {payment.OrderId} khong thanh cong");
            }
        }
    }
}
