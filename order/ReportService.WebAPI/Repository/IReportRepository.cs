using Microsoft.AspNetCore.Mvc;
using ReportService.WebAPI.Dtos;
using ReportService.WebAPI.Models;

namespace ReportService.WebAPI.Repository
{
    public interface IReportRepository
    {
        Task<OrderReportDto> CreateOrderReportAsync(OrderReportDto orderReportDto);
        Task<PaymentReport> CreatePaymentReportAsync(PaymentReportDto paymentReportDto);
        Task<List<RevenueDto>> GetRevenueAsync(ReportFilterDto filter);
        Task<List<OrderStatsDto>> GetOrderStatsAsync(ReportFilterDto filter);
        Task<List<TopProductsDto>> GetTopProductsAsync(ReportFilterDto filter);
        Task<OverviewDto> GetOverviewAsync(DateTime date);
    }
}
