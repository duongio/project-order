using PaymentService.WebAPI.DTOs;

namespace PaymentService.WebAPI.Repository
{
    public interface IPaymentsRepository
    {
        Task<PaymentDto> CreatePaymetAsync(CreatePaymentDto createPaymentDto);
        Task<List<PaymentDto>> GetAllPaymentsAsync();
        Task<PaymentDto?> GetPaymentByIdAsync(Guid id);
        Task<List<PaymentDto>> GetAllPaymentsByOrderIdAsync(Guid orderId);
    }
}
