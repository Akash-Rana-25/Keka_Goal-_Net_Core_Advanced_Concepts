using BankManagment_Domain.Entity;
using BankManagment_Infrastructure.Repository;
using BankManagment_Infrastructure.UnitOfWork;
using Serilog;

namespace BankManagment_Services
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IRepository<PaymentMethod> _paymentMethodRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public PaymentMethodService(IUnitOfWork unitOfWork, IRepository<PaymentMethod> paymentMethodRepository)
        {
            _unitOfWork = unitOfWork;
            _paymentMethodRepository = paymentMethodRepository;
            _logger = Log.ForContext<BankAccountService>();
        }

        public  Task<IEnumerable<PaymentMethod>> GetAllPaymentMethodsAsync()
        {
            _logger.Information("Getting all Payement Method ");
            return _paymentMethodRepository.GetAllAsync();
        }

        public async Task CreatePaymentMethodAsync(PaymentMethod paymentMethod)
        {
            _logger.Information("Creating Payement Method");
            await _paymentMethodRepository.AddAsync(paymentMethod);
        }

        public async Task UpdatePaymentMethodAsync(Guid id, PaymentMethod updatedPaymentMethod)
        {
            var existingPaymentMethod = await _paymentMethodRepository.GetByIdAsync(id);
            if (existingPaymentMethod == null) {
                _logger.Error($"Payement Method Not Found For Update ID {id}");
                throw new ArgumentException("Payment method not found."); 
            }
            existingPaymentMethod.Name = updatedPaymentMethod.Name;
            _logger.Information($"Updated Payement Method For ID {id}");
            await _paymentMethodRepository.UpdateAsync(existingPaymentMethod);
        }

        public async Task DeletePaymentMethodAsync(Guid id)
        {
            var paymentMethod = await _paymentMethodRepository.GetByIdAsync(id);
            if (paymentMethod == null) {
                _logger.Error($"Payement Method Not Found Delete For ID {id}");
                throw new ArgumentException("Payment method not found.");
            }
            _logger.Information($"Deleted Payement Method For ID {id}");
            await _paymentMethodRepository.DeleteAsync(paymentMethod);
        }

        public async Task SaveChangesAsync()
        {
            _logger.Information("save Changes");
            await _unitOfWork.SaveAsync();
        }
    }
}
