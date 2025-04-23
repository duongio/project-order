using Microsoft.EntityFrameworkCore;
using PaymentService.WebAPI.Data;
using PaymentService.WebAPI.DTOs;
using PaymentService.WebAPI.Models;

namespace PaymentService.WebAPI.Repository
{
    public class PaymentsRepository : IPaymentsRepository
    {
        private readonly PaymentDbContext _context;

        public PaymentsRepository(PaymentDbContext context) 
        {
            _context = context;
        }

        public async Task<PaymentDto> CreatePaymetAsync(CreatePaymentDto createPaymentDto)
        {
            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                OrderId = createPaymentDto.OrderId,
                Amount = createPaymentDto.Amount,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Status = (Models.StatusType)createPaymentDto.Status
            };
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
            var paymentDto = new PaymentDto
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                CreatedAt = payment.CreatedAt,
                UpdatedAt = payment.UpdatedAt,
                Status = payment.Status.ToString()
            };
            return paymentDto;
        }

        public async Task<List<PaymentDto>> GetAllPaymentsAsync()
        {
            var payments = await _context.Payments.Select(payment => new PaymentDto
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                CreatedAt = payment.CreatedAt,
                UpdatedAt = payment.UpdatedAt,
                Status = payment.Status.ToString(),
            }).ToListAsync();
            return payments;
        }

        public async Task<List<PaymentDto>> GetAllPaymentsByOrderIdAsync(Guid orderId)
        {
            var payments = await _context.Payments.Where(p => p.OrderId == orderId).ToListAsync();
            var paymentsRes = payments.Select(p => new PaymentDto
            {
                Id = p.Id,
                OrderId = p.OrderId,
                Amount = p.Amount,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                Status = p.Status.ToString(),
            }).ToList();
            return paymentsRes;
        }

        public async Task<PaymentDto?> GetPaymentByIdAsync(Guid id)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.Id == id);
            if (payment == null)
            {
                return null;
            }
            var paymentRes = new PaymentDto
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                CreatedAt = payment.CreatedAt,
                UpdatedAt = payment.UpdatedAt,
                Status = payment.Status.ToString(),
            };
            return paymentRes;
        }
    }
}
