using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReportService.WebAPI.Dtos;
using ReportService.WebAPI.Models;
using ReportService.WebAPI.Repository;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ReportService.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportRepository _reportRepository;

        public ReportsController(IReportRepository ordersRepository)
        {
            _reportRepository = ordersRepository;
        }

        [HttpPost("Order")]
        public async Task<ActionResult<OrderReportDto>> CreateOrderReport([FromBody] OrderReportDto orderReportDto)
        {
            try
            {
                var orderReport = await _reportRepository.CreateOrderReportAsync(orderReportDto);
                if (orderReport != null)
                {
                }
                return Ok(orderReport);
            } catch
            {
                return BadRequest();
            }
        }

        [HttpPost("Payment")]
        public async Task<ActionResult<PaymentReport>> CreatePaymentReport([FromBody] PaymentReportDto paymentReportDto)
        {
            try
            {
                var orderReport = await _reportRepository.CreatePaymentReportAsync(paymentReportDto);
                if (orderReport != null)
                {
                }
                return Ok(orderReport);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("revenue")]
        public async Task<ActionResult<List<RevenueDto>>> GetRevenue([FromQuery] ReportFilterDto filterDto)
        {
            if (filterDto.From > filterDto.To)
            {
                return BadRequest("Ngày bắt đầu phải nhỏ hơn ngày kết thúc.");
            }
            try
            {
                var result = await _reportRepository.GetRevenueAsync(filterDto);
                return Ok(result);
            }
            catch 
            { 
                return BadRequest(); 
            }
        }

        [HttpGet("order-stats")]
        public async Task<ActionResult<List<OrderStatsDto>>> GetOrderStats([FromQuery] ReportFilterDto filterDto)
        {
            if (filterDto.From > filterDto.To)
            {
                return BadRequest("Ngày bắt đầu phải nhỏ hơn ngày kết thúc.");
            }
            try
            {
                var result = await _reportRepository.GetOrderStatsAsync(filterDto);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("top-products")]
        public async Task<ActionResult<List<OrderStatsDto>>> GetTopProducts([FromQuery] ReportFilterDto filterDto)
        {
            if (filterDto.From > filterDto.To)
            {
                return BadRequest("Ngày bắt đầu phải nhỏ hơn ngày kết thúc.");
            }
            try
            {
                var result = await _reportRepository.GetTopProductsAsync(filterDto);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("overview")]
        public async Task<ActionResult<List<OrderStatsDto>>> GetOverview([FromQuery] DateTime date)
        {
            if (date == default)
            {
                return BadRequest("Ngày không hợp lệ.");
            }
            try
            {
                var result = await _reportRepository.GetOverviewAsync(date);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
