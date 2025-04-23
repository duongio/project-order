using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportService.WebAPI.Data;
using ReportService.WebAPI.Dtos;
using ReportService.WebAPI.Models;
using System.Text.RegularExpressions;

namespace ReportService.WebAPI.Repository
{
    public class ReportRepository : IReportRepository
    {
        private readonly ReportDbContext _context;

        public ReportRepository(ReportDbContext context) 
        {
            _context = context;
        }

        public async Task<OrderReportDto> CreateOrderReportAsync(OrderReportDto orderReportDto)
        {
            var orderReport = new OrderReport
            {
                Id = orderReportDto.Id,
                CustomerId = orderReportDto.CustomerId,
                Status = orderReportDto.Status,
                TotalAmount = orderReportDto.TotalAmount,
                CreatedAt = orderReportDto.CreatedAt,
                UpdatedAt = orderReportDto.UpdatedAt
            };
            var orderItemsReport = orderReportDto.Items.Select(itemDto => new OrderItemReport
            {
                Id = itemDto.Id,
                OrderId = orderReport.Id,
                ProductId = itemDto.ProductId,
                Quantity = itemDto.Quantity,
                UnitPrice = itemDto.UnitPrice,
                TotalPrice = itemDto.Quantity * itemDto.UnitPrice
            }).ToList();
            orderReport.Items.AddRange(orderItemsReport);
            orderReport.TotalAmount = orderReport.Items.Sum(item => item.TotalPrice);
            _context.OrdersReport.Add(orderReport);
            _context.OrderItemsReport.AddRange(orderItemsReport);
            await _context.SaveChangesAsync();
            var orderReportDtoRes = new OrderReportDto
            {
                Id = orderReport.Id,
                CustomerId = orderReport.CustomerId,
                Status = orderReport.Status.ToString(),
                TotalAmount = orderReport.TotalAmount,
                CreatedAt = orderReport.CreatedAt,
                UpdatedAt = orderReport.UpdatedAt,
                Items = orderReport.Items.Select(item => new OrderItemReportDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList(),
            };

            return orderReportDtoRes;
        }

        public async Task<PaymentReport> CreatePaymentReportAsync(PaymentReportDto paymentReportDto)
        {
            var paymentReport = new PaymentReport 
            {
                Id = Guid.NewGuid(),
                OrderId = paymentReportDto.OrderId,
                Status = paymentReportDto.Status,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.PaymentReports.Add(paymentReport);
            await _context.SaveChangesAsync();
            return paymentReport;
        }

        public async Task<List<RevenueDto>> GetRevenueAsync(ReportFilterDto filterDto)
        {
            var query = from order in _context.OrdersReport
                        join payment in _context.PaymentReports
                            on order.Id equals payment.OrderId
                        where order.CreatedAt >= filterDto.From
                           && order.CreatedAt <= filterDto.To
                           && payment.Status == "Succeeded"
                        select new
                        {
                            order.CreatedAt,
                            order.TotalAmount
                        };

            var grouped = filterDto.GroupBy == "month"
                ? query.GroupBy(o => new { o.CreatedAt.Year, o.CreatedAt.Month })
                       .Select(g => new RevenueDto
                       {
                           Date = new DateTime(g.Key.Year, g.Key.Month, 1),
                           TotalRevenue = g.Sum(x => x.TotalAmount)
                       })
                : query.GroupBy(o => o.CreatedAt.Date)
                       .Select(g => new RevenueDto
                       {
                           Date = g.Key,
                           TotalRevenue = g.Sum(x => x.TotalAmount)
                       });

            var result = await grouped.OrderBy(x => x.Date).ToListAsync();
            return result;
        }

        public async Task<List<OrderStatsDto>> GetOrderStatsAsync(ReportFilterDto filterDto)
        {
            var query = _context.OrdersReport
                .Where(o => o.CreatedAt >= filterDto.From && o.CreatedAt <= filterDto.To);
            var grouped = filterDto.GroupBy == "month"
                ? query.GroupBy(o => new { o.CreatedAt.Year, o.CreatedAt.Month })
                       .Select(g => new OrderStatsDto
                       {
                           Date = new DateTime(g.Key.Year, g.Key.Month, 1),
                           OrderCount = g.Count()
                       })
                : query.GroupBy(o => o.CreatedAt.Date)
                       .Select(g => new OrderStatsDto
                       {
                           Date = g.Key,
                           OrderCount = g.Count()
                       });
            var result = await grouped.OrderBy(x => x.Date).ToListAsync();
            return result;
        }

        public async Task<List<TopProductsDto>> GetTopProductsAsync(ReportFilterDto filter)
        {
            int top = filter.Limit > 0 ? filter.Limit : 5;

            var query = from item in _context.OrderItemsReport
                        join order in _context.OrdersReport
                            on item.OrderId equals order.Id
                        join payment in _context.PaymentReports
                            on order.Id equals payment.OrderId
                        where order.CreatedAt >= filter.From
                           && order.CreatedAt <= filter.To
                           && payment.Status == "Succeeded"
                        group item by item.ProductId into g
                        select new TopProductsDto
                        {
                            ProductId = g.Key,
                            QuantitySold = g.Sum(x => x.Quantity),
                            TotalRevenue = g.Sum(x => x.Quantity * x.UnitPrice)
                        };

            var result = await query
                .OrderByDescending(x => x.QuantitySold)
                .Take(top)
                .ToListAsync();

            return result;
        }


        public async Task<OverviewDto> GetOverviewAsync(DateTime date)
        {
            var fromDate = date.Date;
            var to = fromDate.AddDays(1);

            var query = from order in _context.OrdersReport
                        join payment in _context.PaymentReports
                            on order.Id equals payment.OrderId
                        where order.CreatedAt >= fromDate
                           && order.CreatedAt < to
                           && payment.Status == "Succeeded"
                        select order;

            var totalRevenue = await query.SumAsync(o => o.TotalAmount);
            var totalOrders = await query.CountAsync();
            var totalCustomers = await query.Select(o => o.CustomerId).Distinct().CountAsync();
            var avgOrderValue = totalOrders > 0 ? totalRevenue / totalOrders : 0;

            var result = new OverviewDto
            {
                Date = date,
                TotalRevenue = totalRevenue,
                TotalOrders = totalOrders,
                TotalCustomers = totalCustomers,
                AverageOrderValue = avgOrderValue
            };

            return result;
        }


    }
}
