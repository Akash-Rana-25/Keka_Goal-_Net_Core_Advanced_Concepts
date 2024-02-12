
using BankManagment_Domain.Entity;
using BankManagment_Infrastructure.Repository;
using BankManagment_Infrastructure.UnitOfWork;
using Serilog;

namespace BankManagment_Services
{

    public class BankAccountService : IBankAccountService
    {
        private readonly IRepository<BankAccount> _bankAccountRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;


        public BankAccountService(IUnitOfWork unitOfWork, IRepository<BankAccount> bankAccountRepository)
        {
            _unitOfWork = unitOfWork;
            _bankAccountRepository = bankAccountRepository;
            _logger = Log.ForContext<BankAccountService>();
        }

        public Task<IEnumerable<BankAccount>> GetAllBankAccountsAsync()
        {
            _logger.Information("Getting all Bank Account");
            return _bankAccountRepository.GetAllAsync();
        }

        public async Task CreateBankAccountAsync(BankAccount bankAccount)
        {
            _logger.Information("Ceating Bank Account");
            await _bankAccountRepository.AddAsync(bankAccount);
        }

        public async Task UpdateBankAccountAsync(Guid id, BankAccount updatedBankAccount)
        {
            var existingBankAccount = await _bankAccountRepository.GetByIdAsync(id);
            if (existingBankAccount == null)
            {
                _logger.Error($"Bank Account Not Found for Update with Id {id}");
                throw new ArgumentException("Bank account not found.");
            }
            existingBankAccount.FirstName = updatedBankAccount.FirstName;
            existingBankAccount.MiddleName = updatedBankAccount.MiddleName;
            existingBankAccount.LastName = updatedBankAccount.LastName;
            existingBankAccount.ClosingDate = updatedBankAccount.ClosingDate;

            _logger.Information($"Bank Account Update For Id {id}");
            await _bankAccountRepository.UpdateAsync(existingBankAccount);
        }

        public async Task DeleteBankAccountAsync(Guid id)
        {
            var bankAccount = await _bankAccountRepository.GetByIdAsync(id);
            if (bankAccount == null) {
                _logger.Error($"Bank Account Not Found for Delete with Id {id}");
                throw new ArgumentException("Bank account not found.");
            }

            _logger.Information($"Bank Account Deleted For Id {id}");
            await _bankAccountRepository.DeleteAsync(bankAccount);
        }

        public async Task SaveChangesAsync()
        {
            _logger.Information("saving Changes");
            await _unitOfWork.SaveAsync();
        }
    }
}