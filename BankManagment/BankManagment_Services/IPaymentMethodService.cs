using BankManagment_Domain.Entity;

namespace BankManagment_Services
{
    public interface IPaymentMethodService
    {
        Task<IEnumerable<PaymentMethod>> GetAllPaymentMethodsAsync();
        Task CreatePaymentMethodAsync(PaymentMethod paymentMethod);
        Task UpdatePaymentMethodAsync(Guid id, PaymentMethod updatedPaymentMethod);
        Task DeletePaymentMethodAsync(Guid id);
        Task SaveChangesAsync();
    }
}
