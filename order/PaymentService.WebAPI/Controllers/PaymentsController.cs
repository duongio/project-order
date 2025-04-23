using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentService.WebAPI.DTOs;
using PaymentService.WebAPI.Repository;

namespace PaymentService.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentsRepository _paymentsRepository;

        public PaymentsController(IPaymentsRepository paymentsRepository) 
        { 
            _paymentsRepository = paymentsRepository;
        }

        [HttpPost]
        public async Task<ActionResult<PaymentDto>> CreatePayment([FromBody] CreatePaymentDto createPaymentDto)
        {
            if (createPaymentDto.PIN == 1111) {
                createPaymentDto.Status = StatusType.Succeeded;
                try
                {
                    var payment = await _paymentsRepository.CreatePaymetAsync(createPaymentDto);
                    return Ok(payment);
                }
                catch
                {
                    return BadRequest();
                }
            }
            else
            {
                createPaymentDto.Status = StatusType.Failed;
                try
                {
                    var payment = await _paymentsRepository.CreatePaymetAsync(createPaymentDto);
                    return Ok(payment);
                }
                catch
                {
                    return BadRequest();
                }
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<PaymentDto>>> GetAllPayments()
        {
            try
            {
                var payments = await _paymentsRepository.GetAllPaymentsAsync();
                return Ok(payments);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<List<PaymentDto>>> GetPaymentsByOrderId(Guid orderId)
        {
            try
            {
                var payments = await _paymentsRepository.GetAllPaymentsByOrderIdAsync(orderId);
                if (payments == null || !payments.Any())
                {
                    return NotFound();
                }
                return Ok(payments);
            } catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDto>> GetPaymentById(Guid id)
        {
            try
            {
                var payment = await _paymentsRepository.GetPaymentByIdAsync(id);
                if (payment == null)
                {
                    return NotFound("Payment not found.");
                }
                return Ok(payment);
            } catch
            {
                return BadRequest();
            }
        }
    }
}
